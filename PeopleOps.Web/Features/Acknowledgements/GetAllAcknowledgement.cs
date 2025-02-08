using System.Text.Json;
using System.Text.Json.Serialization;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Profile;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class GetAllAcknowledgement
{
    public class Query : IRequest<List<AcknowledgementResponse>>
    {
        public bool IncludeSender { get; set; }
    }

    internal sealed class Handler(Client supabaseClient, ISender sender) : IRequestHandler<Query, List<AcknowledgementResponse>>
    {
        private readonly ISender _sender = sender;
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

            if (!request.IncludeSender) return acknowledgements;
            
            // Get sender profile
            foreach (var acknowledgement in acknowledgements)
            {
                var profileResponse = await _sender.Send(new GetProfile.Query { Id = acknowledgement.SenderId }, cancellationToken);
                acknowledgement.Sender = profileResponse;
            }

            return acknowledgements;
        }
    }
}