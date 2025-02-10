using FluentResults;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class GetAcknowledgementLikeByLikerId
{
    public record Query : IRequest<Result<AcknowledgementLikeResponse>>
    {
        public int LikerId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, Result<AcknowledgementLikeResponse>>
    {
        public async Task<Result<AcknowledgementLikeResponse>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var acknowledgementLike = await supabaseClient.From<AcknowledgementLikesTable>()
                .Where(like => like.LikerId == request.LikerId)
                .Single(cancellationToken);

            if (acknowledgementLike is not null)
            {
                return Result.Ok(new AcknowledgementLikeResponse
                {
                    Id = acknowledgementLike.Id,
                    AcknowledgmentId = acknowledgementLike.AcknowledgementId,
                    LikerId = acknowledgementLike.LikerId,
                    CreatedAt = acknowledgementLike.CreatedAt
                });
            }

            return Result.Fail<AcknowledgementLikeResponse>("Acknowledgement like not found");
        }
    }
}