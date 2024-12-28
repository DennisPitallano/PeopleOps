using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class SignOutAttendance
{
    //command to sign out attendance

    public class Command : IRequest<AttendanceResponse>
    {
        public int Id { get; set; }
    }

    // handler to sign out attendance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command,AttendanceResponse>
    {
        public async Task<AttendanceResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var attendance = await supabaseClient.From<AttendanceTable>()
                .Where(p => p.Id == request.Id)
                .Set(x=>x.TimeOut,DateTime.Now)
                .Update(cancellationToken: cancellationToken).ConfigureAwait(false);
            
            var updatedAttendance = attendance.Models.Single();
            //  await attendance.Update<AttendanceTable>(cancellationToken);
            return new AttendanceResponse
            {
                Id = updatedAttendance.Id,
                CreatedAt = updatedAttendance.CreatedAt,
                ActivityDate = updatedAttendance.ActivityDate,
                ProfileId = updatedAttendance.ProfileId,
                TimeIn = updatedAttendance.TimeIn,
                TimeOut = updatedAttendance.TimeOut
            };
        }
    }
    
}