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
        public int LikerId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient, ISender sender)
        : IRequestHandler<Query, List<AcknowledgementResponse>>
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

            if (!request.IncludeSender) return acknowledgements;

            // Get sender profile
            foreach (var acknowledgement in acknowledgements)
            {
                var profileResponse = await sender.Send(new GetProfile.Query { Id = acknowledgement.SenderId },
                    cancellationToken);
                acknowledgement.Sender = profileResponse;

                // get total likes
                var totalLikes = await supabaseClient.Rpc("count_likes_for_acknowledgment",
                    new { acknowledgment_id = acknowledgement.Id });

                if (totalLikes is { ResponseMessage: { IsSuccessStatusCode: true }, Content: not null })
                {
                    acknowledgement.TotalLikes = totalLikes.ResponseMessage is { IsSuccessStatusCode: true }
                        ? int.Parse(totalLikes.Content)
                        : 0;
                }
            }

            return acknowledgements;
        }
    }
}