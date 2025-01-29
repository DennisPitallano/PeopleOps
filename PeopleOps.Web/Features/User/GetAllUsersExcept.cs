using System.Text.Json;
using System.Text.Json.Serialization;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Features.User;

public static class GetAllUsersExcept
{
    public class Query : IRequest<List<ProfileResponse>>
    {
        public int ProfileId { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<ProfileResponse>>
    {
        public async Task<List<ProfileResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<ProfileResponse> profiles = [];
            var baseResponse = await supabaseClient.Rpc("get_all_profiles_except",
                new
                {
                    profileid = request.ProfileId
                }
            ).ConfigureAwait(false);

            if (baseResponse is { ResponseMessage: { IsSuccessStatusCode: true }, Content: not null })
                profiles = JsonSerializer.Deserialize<List<ProfileResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            return profiles;
        }
    }
}