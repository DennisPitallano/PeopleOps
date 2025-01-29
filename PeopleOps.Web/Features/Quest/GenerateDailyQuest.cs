namespace PeopleOps.Web.Features.Quest;

public static class GenerateDailyQuest
{
    //query to generate daily quest
    public class Command : IRequest<bool>
    {
        public int ProfileId { get; set; }
    }

    // handler to generate daily quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var baseResponse = await supabaseClient.Rpc("generate_daily_quest",
                    new { profileid = request.ProfileId, quest_date = DateOnly.FromDateTime(DateTime.Now) })
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage!.IsSuccessStatusCode;
        }
    }
}