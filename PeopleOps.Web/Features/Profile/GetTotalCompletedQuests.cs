namespace PeopleOps.Web.Features.Profile;

public static class GetTotalCompletedQuests
{
    public class Query : IRequest<int>
    {
        public Guid userid { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var baseResponse = await supabaseClient.Rpc("calculate_user_total_completed_quests",
                    new { request.userid })
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage is { IsSuccessStatusCode: true } ? int.Parse(baseResponse.Content) : 0;
        }
    }
    
}