using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using PeopleOps.Web.Helpers;
using PeopleOps.Web.Models;
using Supabase.Gotrue;

namespace PeopleOps.Web.Services;

public class AuthService
{
    private const string AccessToken = nameof(AccessToken);
    private const string RefreshToken = nameof(RefreshToken);

    private readonly NavigationManager _navigation;
    private readonly IConfiguration _configuration;
    private readonly Supabase.Client _supaBaseClient;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly RedisSessionHandler _sessionHandler;
    private readonly HybridCache _memoryCache;

    public AuthService(NavigationManager navigation, IConfiguration configuration, Supabase.Client supaBaseClient,
        ProtectedLocalStorage localStorage, RedisSessionHandler sessionHandler, HybridCache memoryCache)
    {
        _navigation = navigation;
        _configuration = configuration;
        _supaBaseClient = supaBaseClient;
        _localStorage = localStorage;
        _sessionHandler = sessionHandler;
        _memoryCache = memoryCache;
    }

    public async Task<Result> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _supaBaseClient.Auth.SignIn(email, password);
            if (string.IsNullOrEmpty(response?.AccessToken))
            {
                return Result.Fail("An error occured");
            }

            // Save session to memory cache
            await _memoryCache.GetOrCreateAsync($"authSession-{response.User?.Aud}",
                async entry => { return response; });

            return Result.Ok();
        }
        catch (Exception ex)
        {
            var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

            return Result.Fail(GetIgoTrueError(error.error_code));
        }
    }

    public async Task<Result> RegisterAsync(RegisterModel model)
    {
        try
        {
            SignUpOptions signUpOptions = new()
            {
                Data = new Dictionary<string, object>
                {
                    { "first_name", model.FirstName },
                    { "last_name", model.LastName },
                    { "full_name", $"{model.FirstName} {model.LastName}" }
                }
            };

            var data = await _supaBaseClient.Auth.SignUp(model.Email, model.Password, signUpOptions);
            if (data?.AccessToken is not null)
            {
                await LoginAsync(model.Email, model.Password);
            }
            
            // Save session to memory cache
            await _memoryCache.GetOrCreateAsync($"authSession-{model.Email}", async entry => { return data; });

            return Result.Ok();
        }
        catch (Exception ex)
        {
            var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

            return Result.Fail(GetIgoTrueError(error.error_code));
        }
    }

    public async Task<Result> ForgotPasswordAsync(string email)
    {
        ResetPasswordForEmailOptions resetPasswordForEmailOptions = new ResetPasswordForEmailOptions(email)
        {
            RedirectTo = $"{_configuration["RedirectUrl"]}account/reset-password",
        };

        await _supaBaseClient.Auth.ResetPasswordForEmail(resetPasswordForEmailOptions);
        return Result.Ok();
    }

    public async Task<Result<User>> UpdatePassword(ResetPasswordModel model)
    {
        try
        {
            UserAttributes userAttributes = new()
            {
                Password = model.Password
            };

            var session = await _supaBaseClient.Auth.SetSession(model.AccessToken, model.RefreshToken);
            var user = await _supaBaseClient.Auth.Update(userAttributes);

            if (user is null)
                return Result.Fail("An error occured");

            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

            return Result.Fail<User>(GetIgoTrueError(error.error_code));
        }
    }

    private async Task LogoutAsync()
    {
        await _supaBaseClient.Auth.SignOut();

        await RemoveAuthDataFromStorageAsync();
        Thread.Sleep(300);
        _navigation.NavigateTo("/", true);
    }

    public async Task<List<Claim>> GetLoginInfoAsync()
    {
        var emptyResult = new List<Claim>();
        string? accessToken;
        string? refreshToken;
        try
        {
            // Check if user is already logged in
            var session = _supaBaseClient.Auth.CurrentSession;
            if (session != null)
            {
                accessToken = session.AccessToken;
                refreshToken = session.RefreshToken;
            }
            else
            {
                session = await _sessionHandler.LoadSessionAsync();
                accessToken = session?.AccessToken;
                refreshToken = session?.RefreshToken;
            }
        }
        catch (CryptographicException)
        {
            await LogoutAsync();
            return emptyResult;
        }

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return emptyResult;

        var claims = JwtTokenHelper.ValidateDecodeToken(accessToken, _configuration);

        if (claims.Count != 0)
            return claims;

        if (!string.IsNullOrEmpty(refreshToken))
        {
            // TODO : Implement refresh token logic
        }
        else
        {
            await LogoutAsync();
        }

        return claims;
    }

    private async Task RemoveAuthDataFromStorageAsync()
    {
        await _localStorage.DeleteAsync(AccessToken);
        await _localStorage.DeleteAsync(RefreshToken);
    }

    private string GetIgoTrueError(string errorCode)
    {
        string cleanMessage = "";

        switch (errorCode)
        {
            case "user_already_exists":
                cleanMessage = "An account already exists with this email, please log in";
                break;
            case "weak_password":
                cleanMessage =
                    "Password is too weak, please enter at least 6 characters (lowercase, uppercase and numbers)";
                break;
            case "invalid_credentials":
                cleanMessage = "Incorrect email or password";
                break;
            case "same_password":
                cleanMessage = "The new password must be different from the old one";
                break;
            case "email_not_confirmed":
                cleanMessage = "Email not confirmed, please check your inbox";
                break;
        }

        return cleanMessage;
    }
}