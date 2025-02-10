using Microsoft.AspNetCore.Components;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Acknowledgements;

namespace PeopleOps.Web.Components.Pages.Profiles.Feeds;

public partial class Feeds : ComponentBase
{
    private List<AcknowledgementResponse> Acknowledgements { get; set; } = [];
    [CascadingParameter (Name = "Profile")]
    private ProfileResponse Profile { get; set; } 
    [Inject] 
    private ISender Sender { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
     
        await LoadAcknowledgements();
    }
    
    private async Task LoadAcknowledgements()
    {
        var query = new GetAllAcknowledgement.Query { IncludeSender = true };
        Acknowledgements = await Sender.Send(query);
    }
    
    private async Task LikeAcknowledgement(long acknowledgementId)
    {
        var command = new LikeAcknowledgement.Command { LikerId = Profile.Id, AcknowledgementId = acknowledgementId };
        var result = await Sender.Send(command);
        if (result.IsSuccess)
        {
            await LoadAcknowledgements();
        }
    }
   
}