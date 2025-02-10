using FluentResults;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class UnlikeAcknowledgement
{
    public record Command : IRequest<Result>
    {
        public long AcknowledgementLikeId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                await supabaseClient.From<AcknowledgementLikesTable>()
                    .Where(like => like.Id == request.AcknowledgementLikeId)
                    .Delete(cancellationToken: cancellationToken);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail("Acknowledgement like not found");
            }
        }
    }
}