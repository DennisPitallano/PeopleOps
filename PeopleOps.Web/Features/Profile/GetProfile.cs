using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Profile;

public static class GetProfile
{
    //query to get profile

    public class Query : IRequest<ProfileResponse>
    {
        public int Id { get; set; }
    }

    // handler to get profile
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var profiles = await supabaseClient.From<ProfileTable>()
                .Where(p => p.Id == request.Id)
                .Get(cancellationToken).ConfigureAwait(false);
            var profile = profiles.Models.Single();

            return new ProfileResponse
            {
                Id = profile.Id,
                UpdatedAt = profile.UpdatedAt,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email,
                Gender = profile.Gender,
                JobTitle = profile.JobTitle,
                CityAddress = profile.CityAddress,
                FullName = profile.FullName,
                Auth0UserId = profile.Auth0UserId,
                UserName = profile.UserName
            };
        }
    }
}