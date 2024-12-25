using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Components.Pages.Profiles;

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
                CreatedAt = profile.CreatedAt,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email,
                Gender = profile.Gender,
                JobTitle = profile.JobTitle,
                CityAddress = profile.CityAddress
            };
        }
    }
}