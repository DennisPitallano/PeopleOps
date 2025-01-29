using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Acknowledgements;
using PeopleOps.Web.Features.Attendance;
using PeopleOps.Web.Features.Profile;
using PeopleOps.Web.Features.Quest;
using PeopleOps.Web.Features.Tags;
using PeopleOps.Web.Features.User;
using Client = Supabase.Client;

namespace PeopleOps.Web.Components.Pages.Profiles;

public partial class Profile : ComponentBase
{
    #region Parameters

    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    #endregion

    #region Dependencies

    [Inject] private ISender Sender { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] public Client Supabase { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion

    #region Properties

    private ProfileResponse ProfileResponse { get; set; } = new();
    private List<QuestTableResponse> CompletedQuests { get; set; } = [];

    private List<AttendanceTableResponse> AttendanceActivities { get; set; } = [];

    private List<ProfileResponse> Profiles { get; set; } = [];

    private long TotalLedgerPointsBalance { get; set; }
    private int TotalCompletedQuests { get; set; }
    private int TotalTrophies { get; set; }
    private bool IsLoadingData { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        IsLoadingData = true;

        if (AuthenticationState != null)
        {
            var state = await AuthenticationState;
            var profileRequest = SetProfileRequest(state);
            // get or create profile
            var query = new GetOrCreateProfile.Command { ProfileRequest = profileRequest };
            ProfileResponse = await Sender.Send(query);

            TotalLedgerPointsBalance = await GetTotalPoints();
            TotalCompletedQuests = await GetTotalCompletedQuests();
            TotalTrophies = await GetTotalTrophies();

            await LoadCompletedQuests();
            await LoadWeeklyAttendance();
            Profiles = await GetAllProfilesExcept();
        }

        IsLoadingData = false;
    }

    private ProfileRequest SetProfileRequest(AuthenticationState authenticationState)
    {
        return new ProfileRequest
        {
            Auth0UserId = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value,
            FirstName = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
            LastName = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
            Email = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            FullName = authenticationState.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
            AvatarUrl = authenticationState.User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
            UserName = authenticationState.User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
        };
    }

    // load completed quests
    private async Task LoadCompletedQuests()
    {
        var query = new GetCompletedQuest.Query { ProfileId = ProfileResponse.Id };

        CompletedQuests = await Sender.Send(query);
    }

    //load weekly attendance
    private async Task LoadWeeklyAttendance()
    {
        var query = new GetWeeklyAttendanceByUser.Query
        {
            ProfileId = ProfileResponse.Id,
        };

        AttendanceActivities = await Sender.Send(query);
    }

    // get total points ledger balance
    private async Task<int> GetTotalPoints()
    {
        var query = new GetTotalPointLedgerBalance.Query { ProfileId = ProfileResponse.Id };

        return await Sender.Send(query);
    }

    // get total completed quests
    private async Task<int> GetTotalCompletedQuests()
    {
        var query = new GetTotalCompletedQuests.Query { ProfileId = ProfileResponse.Id };

        return await Sender.Send(query);
    }

    // get total trophies
    private async Task<int> GetTotalTrophies()
    {
        var query = new GetTotalAcknowledgements.Query { ReceiverId = ProfileResponse.Id };
        return await Sender.Send(query);
    }

    //get all users except the current user
    private async Task<List<ProfileResponse>> GetAllProfilesExcept()
    {
        var query = new GetAllUsersExcept.Query { ProfileId = ProfileResponse.Id };

        return await Sender.Send(query);
    }

    private async Task OpenQuestCenterDialogAsync()
    {
        DialogParameters parameters = new()
        {
            Title = $"Hello {ProfileResponse.FirstName}",
            TrapFocus = false,
            Modal = true,
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
            Gender = ProfileResponse.Gender ?? true,
            Auth0UserId = ProfileResponse.Auth0UserId,
            Email = ProfileResponse.Email,
            AvatarUrl = ProfileResponse.AvatarUrl,
            FullName = ProfileResponse.FullName,
            UserName = ProfileResponse.UserName
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
            ProfileResponse = (ProfileResponse)result.Data;
        }
    }

    private async Task OpenSendThankYouModalAsync(int receiverId)
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
            Modal = true,
            PreventScroll = true,
            PreventDismissOnOverlayClick = true,
            ShowTitle = false,
            ShowDismiss = false,
            Alignment = HorizontalAlignment.Center,
            PrimaryAction = "",
            SecondaryAction = "Close",
        };

        IDialogReference dialog =
            await DialogService.ShowDialogAsync<SendThankYouModal>(acknowledgementRequest, parameters);
        DialogResult result = await dialog.Result;

        if (result is { Cancelled: true, Data: not null })
        {
            TotalLedgerPointsBalance = await GetTotalPoints();
            TotalCompletedQuests = await GetTotalCompletedQuests();
            TotalTrophies = await GetTotalTrophies();
            await LoadCompletedQuests();
        }
    }
}