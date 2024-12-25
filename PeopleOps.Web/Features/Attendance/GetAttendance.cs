using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class GetAttendance
{
    //query to get attendance

    public class Query : IRequest<AttendanceResponse>
    {
        public int Id { get; set; }
    }

    // handler to get attendance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, AttendanceResponse>
    {
        public async Task<AttendanceResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var attendances = await supabaseClient.From<AttendanceTable>()
                .Where(p => p.Id == request.Id)
                .Get(cancellationToken).ConfigureAwait(false);
            var attendance = attendances.Models.Single();

            return new AttendanceResponse
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                ActivityDate = attendance.ActivityDate,
                ProfileId = attendance.ProfileId,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut
            };
        }
    }
}