namespace PeopleOps.Web.Features.Quest;

public static class GenerateMonthlyQuest
{
    //query to generate monthly quest
    public class Command : IRequest<bool>
    {
        public int ProfileId { get; set; }
    }

    // handler to generate monthly quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var profileid = request.ProfileId;
            var quest_date = DateOnly.FromDateTime(DateTime.UtcNow);
            var baseResponse = await supabaseClient.Rpc("generate_monthly_quest",
                    new { profileid, quest_date })
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage!.IsSuccessStatusCode;
        }
    }
}