using MediatR;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Quest;

public static class SetQuestAsInProgress
{
    //command to set quest to in progress
    public class Command : IRequest<bool>
    {
        public long Id { get; set; }
        public bool Status { get; set; }
    }

    // handler to set quest to in progress
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var userQuests = await supabaseClient.From<UserQuestTable>()
                .Where(p => p.Id == request.Id)
                .Set(x => x.CompletionStatus, request.Status)
                .Update(cancellationToken: cancellationToken).ConfigureAwait(false);
            
            return userQuests.Models.Any();
        }
    }
}