using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace PeopleOps.Web.Services;

public class AuthenticationStateProviderHttp(HttpClient httpClient) : AuthenticationStateProvider
{
    public const string AuthenticationScheme = "Remote";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Api call to retrieve user info: RLS policies, claims etc...
        var userInfoResult =
            await httpClient.GetStringAsync("https://noflvqukvvkoflopttri.supabase.co/auth/v1/user");

        // Mapping the API result to an AuthenticationState object 
        /*
        var authState = userInfoResult.Ok()
            .Map(userInfo => new ClaimsIdentity(userInfo?.GetClaims(), AuthenticationScheme))
            .Map(userIdentity => new ClaimsPrincipal(userIdentity))
            .Map(userPrincipal => new AuthenticationState(userPrincipal))
            .UnwrapOr(new AuthenticationState(new ClaimsPrincipal()));
            */

        return new   AuthenticationState(new ClaimsPrincipal());
    }
}