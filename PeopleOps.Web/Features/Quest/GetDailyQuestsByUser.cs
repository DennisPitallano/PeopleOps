using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using PeopleOps.Web.Contracts;
using Supabase;

namespace PeopleOps.Web.Features.Quest;

public static class GetDailyQuestsByUser
{
    //query to get daily quests
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public Guid userid { get; set; }
        public string questgroup { get; set; }
        public DateOnly quest_date { get; set; }
    }

    // handler to get daily quests
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> dailyQuests = [];
            //get all daily quests for the day
            request.quest_date = DateOnly.FromDateTime(DateTime.Now);
            var baseResponse = await supabaseClient.Rpc("get_daily_user_quests",
                    new { request.quest_date,request.questgroup,request.userid})
                .ConfigureAwait(false);
            //convert the response to a list of daily quests quest_date, questgroup, userid
            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                dailyQuests = JsonSerializer.Deserialize<List<QuestTableResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            }

            return dailyQuests;
        }
    }
}