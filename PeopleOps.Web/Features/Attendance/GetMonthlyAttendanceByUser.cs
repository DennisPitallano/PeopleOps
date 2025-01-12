using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Contracts;
using Supabase;
namespace PeopleOps.Web.Features.Attendance;

public static class GetMonthlyAttendanceByUser
{
    //query to get monthly attendance
    public class Query : IRequest<List<AttendanceTableResponse>>
    {
        public Guid userid { get; set; }
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
    }

    // handler to get monthly attendance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<AttendanceTableResponse>>
    {
        public async Task<List<AttendanceTableResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<AttendanceTableResponse> workingDays = [];
            //var start_of_week = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            request.start_date = DateOnly.FromDateTime(DateTime.Now.StartOfMonth(CultureInfo.CurrentCulture));
            request.end_date = DateOnly.FromDateTime(DateTime.Now.EndOfMonth(CultureInfo.CurrentCulture));
            
            var baseResponse = await supabaseClient.Rpc("get_working_days",
                    new {request.end_date,request.start_date,request.userid})
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
        }
    }
    
}