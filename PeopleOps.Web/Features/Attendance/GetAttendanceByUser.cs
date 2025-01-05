using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class GetAttendanceByUser
{
    //query to get attendance by profile

    public class Query : IRequest<List<AttendanceTableResponse>>
    {
        public Guid userid { get; set; }
    }

    // handler to get attendance by profile
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<AttendanceTableResponse>>
    {
        public async Task<List<AttendanceTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<AttendanceTableResponse> workingDays = [];
            //get all working days for the week
            
            var start_of_week = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var end_of_week = start_of_week.AddDays(6);
            
            var baseResponse = await supabaseClient.Rpc("get_working_days",
                new {request.userid, start_of_week, end_of_week })
                .ConfigureAwait(false);
           //convert the response to a list of dates
           if (baseResponse.ResponseMessage is { IsSuccessStatusCode: true })
           {
                workingDays = JsonSerializer.Deserialize<List<AttendanceTableResponse>>(baseResponse.Content,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true,
                       Converters = { new JsonStringEnumConverter() }
                   })??[];
           }
            
           return workingDays;

           var attendances = await supabaseClient.From<AttendanceTable>()
                .Where(p => p.UserId == request.userid)
                .Get(cancellationToken).ConfigureAwait(false);

            return attendances.Models.Select(attendance => new AttendanceTableResponse
            {
                Id = attendance.Id,
                CreatedAt = attendance.CreatedAt,
                ActivityDate = attendance.ActivityDate.Date,
                UserId = attendance.UserId,
                TimeIn = attendance.TimeIn,
                TimeOut = attendance.TimeOut
            }).OrderBy(d=>d.ActivityDate).ToList();
        }
    }
    
}