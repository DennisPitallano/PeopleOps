namespace PeopleOps.Web.Extensions;

public static class PoDateTimeExtensions
{
    public static List<DateTime> GetWorkingDays(DateTime startDate, DayOfWeek startDay, DayOfWeek endDay)
    {
        var workingDays = new List<DateTime>();
        var currentDate = startDate;

        while (currentDate.DayOfWeek != startDay)
        {
            currentDate = currentDate.AddDays(-1);
        }

        while (currentDate.DayOfWeek != endDay)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                workingDays.Add(currentDate);
            }
            currentDate = currentDate.AddDays(1);
        }

        if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
        {
            workingDays.Add(currentDate);
        }

        return workingDays;
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