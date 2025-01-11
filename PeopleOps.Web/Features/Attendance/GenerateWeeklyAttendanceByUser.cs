using MediatR;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class GenerateWeeklyAttendanceByUser
{
    //query to generate weekly attendance
    public class Command : IRequest<bool>
    {
        public Guid userid { get; set; }
    }

    // handler to generate weekly attendance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
           //get all working days of the week starting from Monday to Friday
            var startOfWeek = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            var attendance = new List<AttendanceTable>();
            for (var i = 0; i < 5; i++)
            {
                var activityDate = startOfWeek.AddDays(i);
                var attendanceTable = new AttendanceTable
                {
                    UserId = request.userid,
                    ActivityDate = DateOnly.FromDateTime(activityDate),
                    CreatedAt = DateTimeOffset.UtcNow.DateTime,
                };
                attendance.Add(attendanceTable);
            }
            var result =  await supabaseClient.From<AttendanceTable>()
                .Insert(attendance, cancellationToken: cancellationToken);
            
            return result.ResponseMessage!.IsSuccessStatusCode;
            
        }
    }
    
}