using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.MonthlyPoints;

public static class GetMonthlyPointsByProfileId
{
    public class Query : IRequest<MonthlyPointsResponse>
    {
        public int ProfileId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, MonthlyPointsResponse>
    {
        public async Task<MonthlyPointsResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = await supabaseClient
                .From<MonthlyPointsTable>()
                .Where(p => p.ProfileId == request.ProfileId)
                .Single(cancellationToken: cancellationToken);

            return new MonthlyPointsResponse
            {
                Id = response.Id,
                CreatedAt = response.CreatedAt,
                MonthYear = response.MonthYear,
                PointsAllocated = response.PointsAllocated,
                PointsSpent = response.PointsSpent,
                ProfileId = response.ProfileId,
                IsRevoke = response.IsRevoke
            };
        }
    }
    
}