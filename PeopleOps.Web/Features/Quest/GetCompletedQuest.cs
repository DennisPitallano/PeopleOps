using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using PeopleOps.Web.Contracts;
using Supabase;

namespace PeopleOps.Web.Features.Quest;

public static class GetCompletedQuest
{
    //query to get completed quest
    public class Query : IRequest<List<QuestTableResponse>>
    {
        public Guid UserId { get; set; }
    }

    // handler to get completed quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<QuestTableResponse>>
    {
        public async Task<List<QuestTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestTableResponse> completedQuests = [];
            var baseResponse = await supabaseClient.Rpc("get_completed_quests",
                new {userid = request.UserId})
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