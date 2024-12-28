using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class GetAttendanceByProfile
{
    //query to get attendance by profile

    public class Query : IRequest<List<AttendanceResponse>>
    {
        public int ProfileId { get; set; }
    }

    // handler to get attendance by profile
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<AttendanceResponse>>
    {
        public async Task<List<AttendanceResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var attendances = await supabaseClient.From<AttendanceTable>()
                .Where(p => p.ProfileId == request.ProfileId)
                .Get(cancellationToken).ConfigureAwait(false);

            return attendances.Models.Select(attendance => new AttendanceResponse
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                ActivityDate = attendance.ActivityDate.Date,
                ProfileId = attendance.ProfileId,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut
            }).OrderBy(d=>d.ActivityDate).ToList();
        }
    }
    
}