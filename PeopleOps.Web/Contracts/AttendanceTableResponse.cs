using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class AttendanceTableResponse 
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("login_time")]
    public DateTime? TimeIn { get; set; }
    
    [JsonPropertyName("logout_time")]
    public DateTime? TimeOut { get; set; }
    
    [JsonPropertyName("activity_date")]
    public DateTimeOffset ActivityDate { get; set; }

    public string DisplayDate => ActivityDate.ToString("MMM dd");
    
    public string DisplayTimeIn => TimeIn?.ToString("hh:mm tt") ?? "N/A";
    
    public string DisplayTimeInId => "time-in-display-" + Id;
    
    public string DisplayTimeOutId => "time-out-display-" + Id;
    
    public string DisplayTimeOut => TimeOut?.ToString("hh:mm tt") ?? "N/A";
    
    public string DisplayTotalHours => TimeIn.HasValue && TimeOut.HasValue ? $"{(TimeOut.Value - TimeIn.Value).TotalHours:0.00} hrs" : "N/A";
    
    public bool IsPresent => TimeIn.HasValue && TimeOut.HasValue;

    public bool IsCurrentDay => ActivityDate.Date == DateTime.Now.Date;
    
    public bool IsMissedThisDay => !IsPresent && ActivityDate.Date < DateTime.Now.Date;
    
    public bool IsFutureDay => ActivityDate.Date > DateTime.Now.Date;

    public string NotCurrentDayStyle => IsCurrentDay ? "padding:15px;" : "padding:15px; opacity: 0.6;";
    
}