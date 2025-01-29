namespace PeopleOps.Web.Features.Quest;

public static class GenerateWeeklyQuest
{
    //query to generate week quest
    public class Command : IRequest<bool>
    {
        public int ProfileId { get; set; }
    }

    // handler to generate week quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var quest_date = DateOnly.FromDateTime(DateTime.Now);
            var baseResponse = await supabaseClient.Rpc("generate_weekly_quest",
                    new { profileid = request.ProfileId,quest_date })
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage!.IsSuccessStatusCode;
        }
    }
}