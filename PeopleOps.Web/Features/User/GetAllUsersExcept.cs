using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.User;

public static class GetAllUsersExcept
{
    public class Query : IRequest<List<UserResponse>>
    {
        public Guid userid { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<UserResponse>>
    {
        public async Task<List<UserResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<UserResponse> users = [];
            var baseResponse = await supabaseClient.Rpc("get_all_users_except", new { request.userid })
                .ConfigureAwait(false);
            
            if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
            {
                users = JsonSerializer.Deserialize<List<UserResponse>>(baseResponse.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? [];
            }
            return users;
        }
    }
}