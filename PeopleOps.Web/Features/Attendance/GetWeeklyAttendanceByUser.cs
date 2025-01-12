using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using PeopleOps.Web.Contracts;
using Supabase;

namespace PeopleOps.Web.Features.Attendance;

public static class GetWeeklyAttendanceByUser
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
            //get all working days for the week from monday to friday
            //var start_of_week = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var monday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            var start_date =DateOnly.FromDateTime(DateTime.Today.StartOfWeek(DayOfWeek.Monday));
            var end_date = DateOnly.FromDateTime(monday.AddDays(4));
            
            var baseResponse = await supabaseClient.Rpc("get_working_days",
                new {end_date,start_date,request.userid})
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