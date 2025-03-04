using System.Globalization;
using FluentResults;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase.Postgrest;
using Client = Supabase.Client;

namespace PeopleOps.Web.Features.MonthlyPoints;

public static class GetMonthlyPointsByProfileId
{
    public class Query : IRequest<Result<MonthlyPointsResponse>>
    {
        public int ProfileId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, Result<MonthlyPointsResponse>>
    {
        public async Task<Result<MonthlyPointsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var monthYear = DateOnly.FromDateTime(DateTime.Now);
            var todayMonth = DateTime.Now.GetMonth(CultureInfo.InvariantCulture);
            var todayYear = DateTime.Now.GetYear(CultureInfo.InvariantCulture);

            var filters = new Dictionary<string, string>
            {
                { "profile_id", request.ProfileId.ToString() },
                { "year", todayYear.ToString() },
                { "month", todayMonth.ToString() },
                {"is_revoke", "false"}
            };

            var response = await supabaseClient
                .From<MonthlyPointsTable>()
                .Match(filters)
                .Single(cancellationToken);

            if (response is not null)   return Result.Ok(MapToMonthlyPointsResponse(response));

            // Create new monthly points
            var newMonthlyPoints = new MonthlyPointsTable
            {
                ProfileId = request.ProfileId,
                MonthYear = monthYear,
                PointsAllocated = 100, //Todo: Get from config
                PointsSpent = 0,
                IsRevoke = false,
                CreatedAt = DateTime.Now,
                Month = todayMonth,
                Year = todayYear
            };

            var baseResponse = await supabaseClient.From<MonthlyPointsTable>()
                .Insert(newMonthlyPoints, new QueryOptions { Returning = QueryOptions.ReturnType.Representation },
                    cancellationToken);
            if (baseResponse.Model is null)
            {
                return Result.Fail<MonthlyPointsResponse>("Failed to create monthly points");
            }
            return Result.Ok(MapToMonthlyPointsResponse(baseResponse.Model));
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
                IsRevoke = points.IsRevoke,
                Month =  points.Month,
                Year =  points.Year
            };
    }
}