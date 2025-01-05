using MediatR;
using Supabase;

namespace PeopleOps.Web.Features.Profile;

public static class GetTotalPoints
{
    
    public class Query : IRequest<int>
    {
        public Guid userid { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        { 
            var totalPoints = await supabaseClient.Rpc<int>("calculate_user_total_points", new { request.userid }).ConfigureAwait(false);
            return totalPoints;
        }
    }
    
}