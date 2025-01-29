using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase.Postgrest;
using Client = Supabase.Client;

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
            var monthYear = DateOnly.FromDateTime(DateTime.Now);
            var response = await supabaseClient
                .From<MonthlyPointsTable>()
                .Where(p => p.ProfileId == request.ProfileId && p.IsRevoke == false)
                .Single(cancellationToken: cancellationToken);

            if (response is not null) return MapToMonthlyPointsResponse(response);
            
            // Create new monthly points
            var newMonthlyPoints = new MonthlyPointsTable
            {
                ProfileId = request.ProfileId,
                MonthYear = monthYear,
                PointsAllocated = 100, //Todo: Get from config
                PointsSpent = 0,
                IsRevoke = false,
                CreatedAt = DateTime.Now
            };

            var baseResponse = await supabaseClient.From<MonthlyPointsTable>()
                .Insert(newMonthlyPoints, new QueryOptions { Returning = QueryOptions.ReturnType.Representation },
                    cancellationToken);

            return MapToMonthlyPointsResponse(baseResponse.Model);

        }

        private static MonthlyPointsResponse MapToMonthlyPointsResponse(MonthlyPointsTable points) =>
            new()
            {
                Id = points.Id,
                CreatedAt = points.CreatedAt,
                MonthYear = points.MonthYear,
                PointsAllocated = points.PointsAllocated,
                PointsSpent = points.PointsSpent,
                ProfileId = points.ProfileId,
                IsRevoke = points.IsRevoke
            };
    }
}