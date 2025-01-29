namespace PeopleOps.Web.Features.Profile;

public static class GetTotalPoints
{
    
    public class Query : IRequest<int>
    {
        public int ProfileId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var profileid = request.ProfileId;
            var totalPoints = await supabaseClient.Rpc<int>("calculate_user_total_points",
                new { profileid }).ConfigureAwait(false);
            return totalPoints;
        }
    }
    
}