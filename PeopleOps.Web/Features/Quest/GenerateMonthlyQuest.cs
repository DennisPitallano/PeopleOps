using MediatR;
using Supabase;

namespace PeopleOps.Web.Features.Quest;

public static class GenerateMonthlyQuest
{
    //query to generate monthly quest
    public class Command : IRequest<bool>
    {
        public Guid userid { get; set; }
    }

    // handler to generate monthly quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var baseResponse = await supabaseClient.Rpc("generate_monthly_quest",
                new {request.userid, quest_date = DateOnly.FromDateTime(DateTime.Now)})
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage!.IsSuccessStatusCode;
        }
    }
    
}