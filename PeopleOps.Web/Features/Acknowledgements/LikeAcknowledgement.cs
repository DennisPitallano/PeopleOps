using FluentResults;
using PeopleOps.Web.Tables;
using Supabase.Postgrest;
using Client = Supabase.Client;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class LikeAcknowledgement
{
    // command to like acknowledgement
    public record Command : IRequest<Result<AcknowledgementLikesTable>>
    {
        public int LikerId { get; set; }
        public long AcknowledgementId { get; set; }
    }

    // handler to like acknowledgement
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, Result<AcknowledgementLikesTable>>
    {
        public async Task<Result<AcknowledgementLikesTable>> Handle(Command request,
            CancellationToken cancellationToken)
        {
            var acknowledgementLikes = new AcknowledgementLikesTable
            {
                AcknowledgementId = request.AcknowledgementId,
                LikerId = request.LikerId,
                CreatedAt = DateTimeOffset.Now,
            };
            var likes = await supabaseClient.From<AcknowledgementLikesTable>()
                .Insert(acknowledgementLikes, new QueryOptions { Returning = QueryOptions.ReturnType.Representation }, cancellationToken).ConfigureAwait(false);

            return likes.ResponseMessage is { IsSuccessStatusCode: true }
                ? Result.Ok(likes.Model?? new AcknowledgementLikesTable())
                : Result.Fail<AcknowledgementLikesTable>("Failed to like acknowledgement");
        }
    }
}