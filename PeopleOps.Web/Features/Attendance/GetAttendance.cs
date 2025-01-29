using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Attendance;

public static class GetAttendance
{
    //query to get attendance

    public class Query : IRequest<AttendanceTableResponse>
    {
        public int Id { get; set; }
    }

    // handler to get attendance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, AttendanceTableResponse>
    {
        public async Task<AttendanceTableResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var attendances = await supabaseClient.From<AttendanceTable>()
                .Where(p => p.Id == request.Id)
                .Get(cancellationToken).ConfigureAwait(false);
            var attendance = attendances.Models.Single();

            return new AttendanceTableResponse
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                ActivityDate = attendance.ActivityDate,
                UserId = attendance.UserId,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut
            };
        }
    }
}