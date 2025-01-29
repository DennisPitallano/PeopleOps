using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Profile;

public static class UpdateProfile
{
    // command to update profile
    public record Command : IRequest<ProfileResponse>
    {
        public ProfileRequest ProfileRequest { get; set; }
    }

    // handler to update profile
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var profile = request.ProfileRequest;
            var profiles = await supabaseClient.From<ProfileTable>()
                .Where(p => p.Id == profile.Id)
                .Set(p => p.FirstName, profile.FirstName)
                .Set(p => p.LastName, profile.LastName)
                .Set(p => p.DateOfBirth, profile.DateOfBirth)
                .Set(p => p.JobTitle, profile.JobTitle)
                .Set(p => p.CityAddress, profile.CityAddress)
                .Set(p => p.Gender, profile.Gender)
                .Update(cancellationToken: cancellationToken).ConfigureAwait(false);

            var updatedProfile = profiles.Models.Single();
            return new ProfileResponse
            {
                Id = updatedProfile.Id,
                FirstName = updatedProfile.FirstName,
                LastName = updatedProfile.LastName,
                DateOfBirth = updatedProfile.DateOfBirth,
                Email = updatedProfile.Email,
                CityAddress = updatedProfile.CityAddress,
                JobTitle = updatedProfile.JobTitle,
                Gender = updatedProfile.Gender,
                Auth0UserId = updatedProfile.Auth0UserId,
                AvatarUrl = updatedProfile.AvatarUrl,
                FullName = updatedProfile.FullName,
                UserName = updatedProfile.UserName,
                UpdatedAt = updatedProfile.UpdatedAt
            };
        }
    }
}