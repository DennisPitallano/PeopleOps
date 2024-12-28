namespace PeopleOps.Web.Contracts;

public class AttendanceResponse
{
    public long Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public long ProfileId { get; set; }
    
    public DateTime? TimeIn { get; set; }
    
    public DateTime? TimeOut { get; set; }
    
    public DateTime ActivityDate { get; set; }

    public string DisplayDate => ActivityDate.ToString("MMMM dd");
    
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