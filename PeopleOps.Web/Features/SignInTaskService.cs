namespace PeopleOps.Web.Services;

public class SignInTaskService
{
    
    // function to get weekly working days
    /*public List<SignInTask> GetWeeklyWorkingDays(string userId)
    {
        var workingDays = new List<SignInTask>();
        var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        var endDate = startDate.AddDays(6);
        var tasks = _context.SignInTasks
            .Where(x => x.UserId == userId && x.SignInTime >= startDate && x.SignInTime <= endDate)
            .OrderBy(x => x.SignInTime)
            .ToList();
        if (tasks.Count > 0)
        {
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                var task = tasks.FirstOrDefault(x => x.SignInTime.Date == currentDate.Date);
                if (task == null)
                {
                    workingDays.Add(new SignInTask
                    {
                        SignInTime = currentDate,
                        SignOutTime = currentDate,
                        UserId = userId
                    });
                }
                else
                {
                    workingDays.Add(task);
                }
                currentDate = currentDate.AddDays(1);
            }
        }
        else
        {
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                workingDays.Add(new SignInTask
                {
                    SignInTime = currentDate,
                    SignOutTime = currentDate,
                    UserId = userId
                });
                currentDate = currentDate.AddDays(1);
            }
        }
        return workingDays;
    }*/
    
}