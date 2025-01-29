namespace PeopleOps.Web.Features.Profile;

public static class GetTotalPointLedgerBalance
{
    //query to get total point ledger balance
    public class Query : IRequest<int>
    {
        public int ProfileId { get; set; }
    }

    // handler to get total point ledger balance
    internal sealed class Handler(Client supabaseClient) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var profileid = request.ProfileId;
            var baseResponse = await supabaseClient.Rpc("calculate_user_total_points_balance",
                    new { profileid })
                .ConfigureAwait(false);
            return baseResponse.ResponseMessage is { IsSuccessStatusCode: true } ? int.Parse(baseResponse.Content) : 0;
        }
    }
}