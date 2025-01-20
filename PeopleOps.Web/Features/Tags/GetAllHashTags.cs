using MediatR;
using PeopleOps.Web.Contracts;
using PeopleOps.Web.Tables;
using Supabase;

namespace PeopleOps.Web.Features.Tags;

public static class GetAllHashTags
{
    public class Query : IRequest<List<TagResponse>>
    {
    }

    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, List<TagResponse>>
    {
        public async Task<List<TagResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var baseResponse = await supabaseClient.From<TagTable>().Get(cancellationToken);
            var tags = baseResponse.Models.Select(x => new TagResponse
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                TagName = x.TagName
            }).ToList();
            return tags;
        }
    }
}