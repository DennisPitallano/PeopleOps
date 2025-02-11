using Microsoft.AspNetCore.Components;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Acknowledgements;

namespace PeopleOps.Web.Components.Pages.Profiles.Feeds;

public partial class Feeds : ComponentBase
{
    private List<AcknowledgementResponse> Acknowledgements { get; set; } = [];
    [Parameter]
    public long ProfileId { get; set; }
    [CascadingParameter(Name = "Profile")] 
    public ProfileResponse Profile { get; set; }
    [Inject] private ISender Sender { get; set; } = null!;
    
    [Inject] private Client SupabaseClient { get; set; } = null!;
    private bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadAcknowledgements();
        IsLoading = false;
    }

    /*protected override async Task OnParametersSetAsync()
    {
        await LoadAcknowledgements();
        await base.OnParametersSetAsync();
    }*/

    private async Task LoadAcknowledgements()
    {
        var query = new GetAllAcknowledgement.Query { IncludeSender = true, LikerId = Profile.Id };
        Acknowledgements = await Sender.Send(query);
    }
    
    private async Task OnLike(AcknowledgementResponse acknowledgement)
    {
        // get total likes
        var totalLikes = await SupabaseClient.Rpc("count_likes_for_acknowledgment",
            new { acknowledgment_id = acknowledgement.Id });

        if (totalLikes is { ResponseMessage: { IsSuccessStatusCode: true }, Content: not null })
        {
            acknowledgement.TotalLikes = totalLikes.ResponseMessage is { IsSuccessStatusCode: true }
                ? int.Parse(totalLikes.Content)
                : 0;
        }
    }
}