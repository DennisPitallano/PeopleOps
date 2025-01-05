using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using PeopleOps.Web.Services;
using Supabase;

namespace PeopleOps.Web.Providers;

public class SupabaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;
    private readonly Client _client;
    public SupabaseAuthenticationStateProvider(AuthService authService, Client client)
    {
        _authService = authService;
        _client = client;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var supabaseUser =  _client.Auth.CurrentUser;
            var supabaseSession = _client.Auth.CurrentSession;
            
            var claims = await _authService.GetLoginInfoAsync();
            var claimsIdentity = claims.Count != 0
                ? new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, "name", "role")
                : new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationState(claimsPrincipal);
        }
        catch (Exception)
        {
            // very bad but fix issue with javascript not available
            return new AuthenticationState(new ClaimsPrincipal());
        }
    }
}
