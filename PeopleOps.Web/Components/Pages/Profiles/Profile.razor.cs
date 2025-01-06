using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Profile;
using Supabase;

namespace PeopleOps.Web.Components.Pages.Profiles;


public partial class Profile : ComponentBase
{
    [Inject]
    private ISender Sender { get; set; }

    [Inject]
    private IDialogService DialogService { get;set; }
    private ProfileResponse? ProfileResponse { get; set; }
    
    [Inject]
    public Client Supabase { get; set; }
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var user = Supabase.Auth.CurrentUser;
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        // send get query profile
        var query = new GetProfile.Query { Id = Guid.Parse(user?.Id) };
        var totalPoints = await Sender.Send(new GetTotalPoints.Query { userid = Guid.Parse("75588cf8-3246-4ff3-a768-aa0f8020678d") });
        ProfileResponse = await Sender.Send(query);
    }
}