using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Extensions;

namespace PeopleOps.Web.Features.Quest;

public static class GetWeeklyQuestsByUser
{
    //query to get weekly quests
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public int ProfileId { get; set; }
    }

    // handler to get weekly quests
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> weeklyQuests = [];
            //get all weekly quests for the day
            // set the start and end date for the week to get the weekly quests
            var start_date = DateOnly.FromDateTime(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            var end_date = DateOnly.FromDateTime(DateTime.Now.EndOfWeek(DayOfWeek.Friday));

            // call the get_weekly_quests rpc

            var baseResponse = await supabaseClient.Rpc("get_weekly_quests",
                    new
                    {
                        end_date,
                        profileid = request.ProfileId,
                        start_date
                    })
                .ConfigureAwait(false);
            
            //convert the response to a list of weekly quests quest_date, questgroup, userid
            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                weeklyQuests = JsonSerializer.Deserialize<List<QuestTableResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            }

            return weeklyQuests;
        }
    }
}