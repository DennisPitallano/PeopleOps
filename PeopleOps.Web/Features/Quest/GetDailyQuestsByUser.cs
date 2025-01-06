using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using PeopleOps.Web.Contracts;
using Supabase;

namespace PeopleOps.Web.Features.Quest;

public static class GetDailyQuestsByUser
{
    //query to get daily quests
    public class Query : IRequest<List<DailyQuestTableResponse>>
    {
        public Guid userid { get; set; }
        public string questgroup { get; set; }
        public DateTime quest_date { get; set; }
    }

    // handler to get daily quests
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<DailyQuestTableResponse>>
    {
        public async Task<List<DailyQuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<DailyQuestTableResponse> dailyQuests = [];
            //get all daily quests for the day
            request.quest_date = DateTime.UtcNow.Date;
            var baseResponse = await supabaseClient.Rpc("get_daily_user_quests",
                    new { request.quest_date,request.questgroup,request.userid})
                .ConfigureAwait(false);
            //convert the response to a list of daily quests quest_date, questgroup, userid
            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                dailyQuests = JsonSerializer.Deserialize<List<DailyQuestTableResponse>>(baseResponse.Content,
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