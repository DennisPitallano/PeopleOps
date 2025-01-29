using System.Text.Json;
using System.Text.Json.Serialization;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class GetAllAcknowledgement
{
    public class Query : IRequest<List<AcknowledgementResponse>>
    {
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<AcknowledgementResponse>>
    {
        public async Task<List<AcknowledgementResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<AcknowledgementResponse> acknowledgements = [];

            var baseResponse = await supabaseClient.Rpc("get_all_acknowledgments",
                    new { })
                .ConfigureAwait(false);

            if (baseResponse is { ResponseMessage: { IsSuccessStatusCode: true }, Content: not null })
                acknowledgements = JsonSerializer.Deserialize<List<AcknowledgementResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            return acknowledgements;
        }
    }
}