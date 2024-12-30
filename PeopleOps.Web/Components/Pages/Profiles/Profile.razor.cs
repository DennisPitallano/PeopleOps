using MediatR;
using Microsoft.AspNetCore.Components;
using PeopleOps.Web.Contracts;

namespace PeopleOps.Web.Components.Pages.Profiles;


public partial class Profile : ComponentBase
{
    [Inject]
    private ISender Sender { get; set; }

    [Inject]
    private IDialogService DialogService { get;set; }
    private ProfileResponse? ProfileResponse { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // send get query profile
        var query = new GetProfile.Query { Id = 1 };
        ProfileResponse = await Sender.Send(query);
    }

   
}