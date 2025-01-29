using System.Text.Json;
using System.Text.Json.Serialization;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Features.Quest;

public static class GetDailyQuestsByUser
{
    //query to get daily quests
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public int ProfileId { get; set; }
        public string QuestGroup { get; set; }
        public DateOnly QuestDate { get; set; }
    }

    // handler to get daily quests
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> dailyQuests = [];
            //get all daily quests for the day
            request.QuestDate = DateOnly.FromDateTime(DateTime.Now);
            var baseResponse = await supabaseClient.Rpc("get_daily_user_quests",
                    new
                    {
                        profileid = request.ProfileId,
                        quest_date = request.QuestDate,
                        questgroup = request.QuestGroup
                    })
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