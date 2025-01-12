using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Features.Attendance;
using PeopleOps.Web.Features.Profile;
using PeopleOps.Web.Features.Quest;
using Supabase.Gotrue;
using Client = Supabase.Client;

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
    
    private List<QuestTableResponse> CompletedQuests { get; set; } = [];
    
    private List<AttendanceTableResponse> AttendanceActivities { get; set; } = [];
    
    private long TotalLedgerPointsBalance { get; set; }
    private int TotalCompletedQuests { get; set; }
    
    User? User { get; set; }
    protected override async Task OnInitializedAsync()
    {
        User = Supabase.Auth.CurrentUser;
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        // send get query profile
        var query = new GetProfile.Query { Id = Guid.Parse(User?.Id) };
        
        TotalLedgerPointsBalance = await GetTotalPoints();
        TotalCompletedQuests = await GetTotalCompletedQuests();
        ProfileResponse = await Sender.Send(query);
        
        await LoadCompletedQuests();
        await LoadWeeklyAttendance();
    }
    
    // load completed quests
    private async Task LoadCompletedQuests()
    {
        var query = new GetCompletedQuest.Query { UserId = Guid.Parse(User?.Id) };
        
        CompletedQuests = await Sender.Send(query);
    }
    
    //load weekly attendance
    private async Task LoadWeeklyAttendance()
    {
        var query = new GetWeeklyAttendanceByUser.Query
        {
            userid = Guid.Parse(User?.Id)
        };
        
        AttendanceActivities = await Sender.Send(query);
    }
    
    // get total points ledger balance
    private async Task<int> GetTotalPoints()
    {
        var user = Supabase.Auth.CurrentUser;
        var query = new GetTotalPointLedgerBalance.Query { userid = Guid.Parse(user?.Id) };
        
        return await Sender.Send(query);
    }
    
    // get total completed quests
    private async Task<int> GetTotalCompletedQuests()
    {
        var user = Supabase.Auth.CurrentUser;
        var query = new GetTotalCompletedQuests.Query { userid = Guid.Parse(user?.Id) };
        
        return await Sender.Send(query);
    }
    
}