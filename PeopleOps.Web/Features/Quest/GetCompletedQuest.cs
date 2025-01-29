using System.Text.Json;
using System.Text.Json.Serialization;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Features.Quest;

public static class GetCompletedQuest
{
    //query to get completed quest
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public int ProfileId { get; set; }
    }

    // handler to get completed quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> completedQuests = [];
            var baseResponse = await supabaseClient.Rpc("get_completed_quests",
                    new { profileid = request.ProfileId })
                .ConfigureAwait(false);

            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                completedQuests = JsonSerializer.Deserialize<List<QuestTableResponse>>(baseResponse.Content!,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            }

            return completedQuests;
        }
    }
}