using FluentResults;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class GetAcknowledgementPointsByAcknowledgementId
{
    public class Query : IRequest<Result<List<AcknowledgementPointsResponse>>>
    {
        public long AcknowledgementId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient)
        : IRequestHandler<Query, Result<List<AcknowledgementPointsResponse>>>
    {
        public async Task<Result<List<AcknowledgementPointsResponse>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var result = await supabaseClient
                .From<AcknowledgementPointsTable>()
                .Where(ap => ap.AcknowledgmentId == request.AcknowledgementId)
                .Get(cancellationToken);

            if (result.Models.Count == 0)
            {
                return Result.Fail<List<AcknowledgementPointsResponse>>("No acknowledgment points found.");
            }

            var acknowledgmentPoints = result.Models.Select(ap => new AcknowledgementPointsResponse
            {
                Id = ap.Id,
                AcknowledgmentId = ap.AcknowledgmentId,
                ReceiverId = ap.ReceiverId,
                PointsEarned = ap.Coins,
                CreatedAt = ap.CreatedAt
            }).ToList();

            return Result.Ok(acknowledgmentPoints);
        }
    }
}