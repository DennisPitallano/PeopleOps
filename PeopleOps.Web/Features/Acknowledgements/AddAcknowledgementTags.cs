using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;

namespace PeopleOps.Web.Features.Acknowledgements;

public static class AddAcknowledgementTags
{
    public class Command : IRequest<AcknowledgementTagResponse>
    {
        public List<AcknowledgementTagRequest>? AcknowledgementTagRequests { get; set; }
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Command, AcknowledgementTagResponse>
    {
        public async Task<AcknowledgementTagResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            foreach (var tagRequest in request.AcknowledgementTagRequests)
            {
                var acknowledgmentTag = new AcknowledgementTagTable
                {
                    AcknowledgmentId = tagRequest.AcknowledgmentId,
                    TagId = tagRequest.TagId,
                    CreatedAt = DateTime.Now
                };

                await supabaseClient
                    .From<AcknowledgementTagTable>()
                    .Insert(acknowledgmentTag, cancellationToken: cancellationToken);
                
            }
            
            return new AcknowledgementTagResponse();
        }
    }
    
}