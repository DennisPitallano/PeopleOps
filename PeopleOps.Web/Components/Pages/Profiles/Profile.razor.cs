using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Attendance;
using PeopleOps.Web.Features.Profile;
using PeopleOps.Web.Features.Quest;
using PeopleOps.Web.Features.Tags;
using PeopleOps.Web.Features.User;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace PeopleOps.Web.Components.Pages.Profiles;

public partial class Profile : ComponentBase
{
    [Inject] private ISender Sender { get; set; }

    [Inject] private IDialogService DialogService { get; set; }
    private ProfileResponse ProfileResponse { get; set; } = new();

    [Inject] public Client Supabase { get; set; }
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private List<QuestTableResponse> CompletedQuests { get; set; } = [];

    private List<AttendanceTableResponse> AttendanceActivities { get; set; } = [];
    
    private List<UserResponse> Users { get; set; } = [];

    private long TotalLedgerPointsBalance { get; set; }
    private int TotalCompletedQuests { get; set; }

    private bool IsLoadingData { get; set; }

    bool DeferredLoading = false;
    private bool _modal = true;
    User? User { get; set; }
    string? UserGuid { get; set; }
    protected override async Task OnInitializedAsync()
    {
        IsLoadingData = true;
       // User = Supabase.Auth.CurrentUser;
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        UserGuid = state.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        // send get query profile
        var query = new GetProfile.Query { Id = Guid.Parse(UserGuid) };

        TotalLedgerPointsBalance = await GetTotalPoints();
        TotalCompletedQuests = await GetTotalCompletedQuests();
        ProfileResponse = await Sender.Send(query);

        await LoadCompletedQuests();
        await LoadWeeklyAttendance();
        Users = await GetAllUsersExcept();
        IsLoadingData = false;
    }

    // load completed quests
    private async Task LoadCompletedQuests()
    {
        var query = new GetCompletedQuest.Query { UserId = Guid.Parse(UserGuid) };

        CompletedQuests = await Sender.Send(query);
    }

    //load weekly attendance
    private async Task LoadWeeklyAttendance()
    {
        var query = new GetWeeklyAttendanceByUser.Query
        {
            userid = Guid.Parse(UserGuid)
        };

        AttendanceActivities = await Sender.Send(query);
    }

    // get total points ledger balance
    private async Task<int> GetTotalPoints()
    {
        var query = new GetTotalPointLedgerBalance.Query { userid = Guid.Parse(UserGuid) };

        return await Sender.Send(query);
    }

    // get total completed quests
    private async Task<int> GetTotalCompletedQuests()
    {
        var query = new GetTotalCompletedQuests.Query { userid = Guid.Parse(UserGuid) };

        return await Sender.Send(query);
    }
    
    //get all users except the current user
    private async Task<List<UserResponse>> GetAllUsersExcept()
    {
        var query = new GetAllUsersExcept.Query { userid = Guid.Parse(UserGuid) };

        return await Sender.Send(query);
    }

    private async Task OpenQuestCenterDialogAsync()
    {
        DialogParameters parameters = new()
        {
            Title = $"Hello {ProfileResponse.FirstName}",
            TrapFocus = false,
            Modal = _modal,
            PreventScroll = true,
            PreventDismissOnOverlayClick = true,
            ShowTitle = false,
            ShowDismiss = false,
            Alignment = HorizontalAlignment.Center,
            PrimaryAction = "",
            SecondaryAction = "Close",
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<TaskCenterModal>(ProfileResponse, parameters);
        DialogResult result = await dialog.Result;

        if (result is { Cancelled: true, Data: not null })
        {
            await dialog.CloseAsync();
            TotalLedgerPointsBalance = await GetTotalPoints();
            TotalCompletedQuests = await GetTotalCompletedQuests();
            await LoadCompletedQuests();
        }
    }

    private async Task OpenCompleteProfileModalAsync()
    {
        ProfileRequest profileRequest = new()
        {
            Id = ProfileResponse.Id,
            FirstName = ProfileResponse.FirstName,
            LastName = ProfileResponse.LastName,
            DateOfBirth = ProfileResponse.DateOfBirth,
            CityAddress = ProfileResponse.CityAddress,
            JobTitle = ProfileResponse.JobTitle,
            Gender = ProfileResponse.Gender?? true
        };
        
        DialogParameters parameters = new()
        {
            Title = $"Hello {ProfileResponse.FirstName}",
            Height = "550px",
            Width = "400px",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        };

        IDialogReference dialog =
            await DialogService.ShowDialogAsync<CompleteProfileModal>(profileRequest, parameters);
        DialogResult result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
          //  var updatedProfile = (ProfileRequest)result.Data;
            //update profile
           // var command = new UpdateProfile.Command { ProfileRequest = updatedProfile };
           ProfileResponse =  (ProfileResponse)result.Data;
        }
    }
    
    private async Task OpenSendThankYouModalAsync(Guid receiverId)
    {
        
        var acknowledgementTagsQuery = new GetAllHashTags.Query();
        var acknowledgementTags = await Sender.Send(acknowledgementTagsQuery);
        
        AcknowledgementRequest acknowledgementRequest = new()
        { 
            ReceiverId = receiverId,
            SenderId = ProfileResponse.Id,
            AcknowledgmentDate = DateTime.Now,
            AcknowledgementTags = acknowledgementTags,
            Message = "Thank you for your hard work! 🙏 🎉",
            ReceiverList = [receiverId],
        };
        
        DialogParameters parameters = new()
        {
            Title = $"Hello {ProfileResponse.FirstName}",
            TrapFocus = false,
            Modal = _modal,
            PreventScroll = true,
            PreventDismissOnOverlayClick = true,
            ShowTitle = false,
            ShowDismiss = false,
            Alignment = HorizontalAlignment.Center,
            PrimaryAction = "",
            SecondaryAction = "Close",
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<SendThankYouModal>(acknowledgementRequest, parameters);
        DialogResult result = await dialog.Result;

        if (result is { Cancelled: true, Data: not null })
        {
            TotalLedgerPointsBalance = await GetTotalPoints();
            TotalCompletedQuests = await GetTotalCompletedQuests();
            await LoadCompletedQuests();
        }
    }
    
}