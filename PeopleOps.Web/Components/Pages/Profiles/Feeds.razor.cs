using MediatR;
using Microsoft.AspNetCore.Components;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Acknowledgements;

namespace PeopleOps.Web.Components.Pages.Profiles;

public partial class Feeds : ComponentBase
{
    private List<AcknowledgementResponse> Acknowledgements { get; set; } = [];
    [Inject] 
    private ISender Sender { get; set; } = null!;
    
    
    protected override async Task OnInitializedAsync()
    {
        await LoadAcknowledgements();
        await base.OnInitializedAsync();
    }
    
    private async Task LoadAcknowledgements()
    {
        var query = new GetAllAcknowledgement.Query();
        
        Acknowledgements = await Sender.Send(query);
    }
}