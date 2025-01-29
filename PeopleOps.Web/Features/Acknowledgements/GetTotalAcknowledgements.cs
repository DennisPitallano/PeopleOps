using System.Text.Json;
using System.Text.Json.Serialization;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class GetTotalAcknowledgements
{
    public class Query : IRequest<int>
    {
        public int ReceiverId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var profileid  = request.ReceiverId;
            var baseResponse = await supabaseClient.Rpc("get_total_acknowledgements_points",
                    new { profileid })
                .ConfigureAwait(false);

            if (baseResponse is { ResponseMessage: { IsSuccessStatusCode: true }, Content: not null })
                return JsonSerializer.Deserialize<int>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    });

            return 0;
        }
    }
    
}