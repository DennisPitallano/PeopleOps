using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Profile;

public static class GetOrCreateProfile
{
    public class Command : IRequest<ProfileResponse>
    {
        public required ProfileRequest ProfileRequest { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var profile = request.ProfileRequest;
            // check first if profile exist
            var existingProfile = await supabaseClient.From<ProfileTable>()
                .Where(p => p.Email == profile.Email && p.UserName == profile.UserName)
                .Single(cancellationToken: cancellationToken);

           
            if (existingProfile != null)
            {
                return MapToProfileResponse(existingProfile);
            }

            var newProfile = new ProfileTable
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email,
                CityAddress = profile.CityAddress,
                JobTitle = profile.JobTitle,
                AvatarUrl = profile.AvatarUrl ?? GenerateAvatarUrl(profile.FirstName, profile.LastName),
                Gender = null,
                UpdatedAt = DateTime.UtcNow,
                FullName = profile.FullName ?? profile.Email,
                UserName = profile.UserName ?? profile.Email,
                Auth0UserId = profile.Auth0UserId
            };

            var profiles = await supabaseClient.From<ProfileTable>()
                .Insert(newProfile, cancellationToken: cancellationToken);

            return MapToProfileResponse(profiles.Models.Single());
        }
        
        private static ProfileResponse MapToProfileResponse(ProfileTable profile) => new ProfileResponse
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            DateOfBirth = profile.DateOfBirth,
            Email = profile.Email,
            CityAddress = profile.CityAddress,
            JobTitle = profile.JobTitle,
            Gender = profile.Gender,
            AvatarUrl = profile.AvatarUrl,
            FullName = profile.FullName,
            UpdatedAt = profile.UpdatedAt,
            UserName = profile.UserName,
            Auth0UserId = profile.Auth0UserId
        };

        private static string GenerateAvatarUrl(string? firstName, string? lastName) =>
            $"https://ui-avatars.com/api/?name={firstName}+{lastName}&background=random&color=fff&size=128";
    }
}