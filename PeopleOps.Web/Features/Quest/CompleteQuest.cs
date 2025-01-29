using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Quest;

public static class CompleteQuest
{
    //command to complete quest
    public class Command : IRequest<bool>
    {
        public long Id { get; set; }
    }

    // handler to complete quest
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var userQuests = await supabaseClient.From<UserQuestTable>()
                .Where(p => p.Id == request.Id)
                .Set(x => x.CompletionStatus, true)
                .Set(x => x.CompletionDate, DateTime.Now)
                .Update(cancellationToken: cancellationToken).ConfigureAwait(false);
            
            return userQuests.Models.Any();
        }
    }
    
}