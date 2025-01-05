using MediatR;
using Supabase;

namespace PeopleOps.Web.Features.Profile;

public static class GenerateDailyQuest
{
    //query to generate daily quest
    public class Command : IRequest<bool>
    {
        public Guid userid { get; set; }
    }

    // handler to generate daily quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var baseResponse = await supabaseClient.Rpc("generate_daily_quest",
                new {request.userid})
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage.IsSuccessStatusCode;
        }
    }
    
}