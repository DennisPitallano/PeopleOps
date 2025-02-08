using FluentResults;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.MonthlyPoints;
using PeopleOps.Web.Tables;
using Supabase.Postgrest;
using Client = Supabase.Client;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class SendAcknowledgement
{
    public class Command : IRequest<AcknowledgementResponse>
    {
        public required AcknowledgementRequest AcknowledgementRequest { get; set; }
    }

    internal sealed class Handler(Client supabaseClient, ISender sender) : IRequestHandler<Command, AcknowledgementResponse>
    {
        private readonly ISender _sender = sender;
        public async Task<AcknowledgementResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var acknowledgment = new AcknowledgmentTable
            {
                SenderId = request.AcknowledgementRequest.SenderId,
                AcknowledgmentDate = request.AcknowledgementRequest.AcknowledgmentDate,
                Message = request.AcknowledgementRequest.Message,
                CreatedAt = DateTime.Now
            };

            var result = await supabaseClient
                .From<AcknowledgmentTable>()
                .Insert(acknowledgment, new QueryOptions { Returning = QueryOptions.ReturnType.Representation },
                    cancellationToken);

            AcknowledgmentTable? insertedAcknowledgment = result.Model;

            if (insertedAcknowledgment is null)
            {
                return new AcknowledgementResponse();
            }
            // insert acknowledgment tags
            List<AcknowledgementTagTable> acknowledgmentTags = request.AcknowledgementRequest.TagIds.Select(tag =>
                new AcknowledgementTagTable { AcknowledgmentId = insertedAcknowledgment.Id, TagId = tag, }).ToList();

            await supabaseClient
                .From<AcknowledgementTagTable>()
                .Insert(acknowledgmentTags, cancellationToken: cancellationToken);

            // insert acknowledgment points
            List<AcknowledgementPointsTable> acknowledgmentPoints = request.AcknowledgementRequest.ReceiverList
                .Select(receiver => new AcknowledgementPointsTable
                {
                    AcknowledgmentId = insertedAcknowledgment.Id, ReceiverId = receiver,
                    Coins = request.AcknowledgementRequest.Coins
                }).ToList();

            await supabaseClient
                .From<AcknowledgementPointsTable>()
                .Insert(acknowledgmentPoints, cancellationToken: cancellationToken);

            // update monthly points
            var totalCoins = acknowledgmentPoints.Sum(x => x.Coins);

            // get current monthly points
            var getMonthlyPointQuery = new GetMonthlyPointsByProfileId.Query
            {
                ProfileId =  request.AcknowledgementRequest.SenderId
            };
            
             Result<MonthlyPointsResponse>  availableMonthlyPoints = await _sender.Send(getMonthlyPointQuery, cancellationToken);

            // update monthly points
            await supabaseClient
                .From<MonthlyPointsTable>()
                .Where(p => p.Id == availableMonthlyPoints.Value.Id)
                .Set(x => x.PointsSpent, availableMonthlyPoints.Value.PointsSpent + totalCoins)
                .Update(cancellationToken: cancellationToken).ConfigureAwait(false);

            return new AcknowledgementResponse
            {
                Id = insertedAcknowledgment.Id,
                CreatedAt = insertedAcknowledgment.CreatedAt,
                SenderId = insertedAcknowledgment.SenderId,
                AcknowledgmentDate = insertedAcknowledgment.AcknowledgmentDate,
                Message = insertedAcknowledgment.Message
            };
        }
    }
}