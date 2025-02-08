namespace PeopleOps.Web.Extensions;

public static class PoDateTimeExtensions
{
 
    public static string Humanize(this DateTime dt)
    {
        var timeSpan = DateTime.Now.Subtract(dt);
        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            return "Just now";
        }
        if (timeSpan <= TimeSpan.FromMinutes(1))
        {
            return timeSpan.Seconds + " seconds ago";
        }
        if (timeSpan <= TimeSpan.FromMinutes(2))
        {
            return "1 minute ago";
        }
        if (timeSpan <= TimeSpan.FromHours(1))
        {
            return timeSpan.Minutes + " minutes ago";
        }
        if (timeSpan <= TimeSpan.FromHours(2))
        {
            return "1 hr ago";
        }
        if (timeSpan <= TimeSpan.FromDays(1))
        {
            return timeSpan.Hours + " hours ago";
        }
        if (timeSpan <= TimeSpan.FromDays(2))
        {
            return "yesterday"; 
        }
        if (timeSpan <= TimeSpan.FromDays(7))
        {
            return timeSpan.Days + " days ago";
        }
        if (timeSpan <= TimeSpan.FromDays(14))
        {
            return "last week";
        }
        if (timeSpan <= TimeSpan.FromDays(21))
        {
            return "2 weeks ago";
        }
        if (timeSpan <= TimeSpan.FromDays(28))
        {
            return "3 weeks ago";
        }
        if (timeSpan <= TimeSpan.FromDays(60))
        {
            return "last month";
        }
        if (timeSpan <= TimeSpan.FromDays(365))
        {
            return timeSpan.Days / 30 + " months ago";
        }
        if (timeSpan <= TimeSpan.FromDays(730))
        {
            return "last year";
        }
        return timeSpan.Days / 365 + " years ago";
    }
    
    public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
    {
        int diff = endOfWeek - dt.DayOfWeek;
        if (diff < 0)
        {
            diff += 7;
        }
        return dt.AddDays(diff);
    }
}