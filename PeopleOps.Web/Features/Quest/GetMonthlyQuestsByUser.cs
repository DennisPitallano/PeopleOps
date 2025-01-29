using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Features.Quest;

public static class GetMonthlyQuestsByUser
{
    //query to get monthly quests
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public Guid userid { get; set; }
        public string questgroup { get; set; }
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
    }

    // handler to get monthly quests
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> monthlyQuests = [];
            //get all monthly quests for the day
            // set the start and end date for the month to get the monthly quests
            request.start_date = DateOnly.FromDateTime(DateTime.Now.StartOfMonth(CultureInfo.CurrentCulture));
            request.end_date = DateOnly.FromDateTime(DateTime.Now.EndOfMonth(CultureInfo.CurrentCulture));
            
            // call the get_monthly_quests rpc
            
            var baseResponse = await supabaseClient.Rpc("get_monthly_quests",
                    new { request.end_date,request.start_date,request.userid})
                .ConfigureAwait(false);
            //convert the response to a list of monthly quests quest_date, questgroup, userid
            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                monthlyQuests = JsonSerializer.Deserialize<List<QuestTableResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            }

            return monthlyQuests;
        }
    }
    
}