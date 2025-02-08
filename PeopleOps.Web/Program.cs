using Auth0.AspNetCore.Authentication;
using Blazored.LocalStorage;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using PeopleOps.Web.Components.Pages.Account;
using PeopleOps.Web.Extensions;
using PeopleOps.Web.Identity;
using PeopleOps.Web.Providers;
using PeopleOps.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddBlazoredLocalStorage();
// Authenticated services
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<ILocalStorageServices, LocalStorageService>();
builder.Services.AddScoped<RedisSessionHandler>();
builder.Services.AddScoped<SupabaseAuthService>();
builder.Services.AddScoped<UserService>();

// register supabase client
var url = builder.Configuration["SUPABASE_URL"];
var key = builder.Configuration["SUPABASE_KEY"];
var options = new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
    //SessionHandler = new InMemorySessionHandler()
    //SessionHandler = new RedisSessionHandler()
    
};
// Note the creation as a singleton.
builder.Services.AddScoped(_ => new Client(url!, key, options));

//builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuth0WebAppAuthentication(appOptions =>
{
    appOptions.Domain = builder.Configuration["Auth0:Domain"]!;
    appOptions.ClientId = builder.Configuration["Auth0:ClientId"]!;
    appOptions.Scope = "openid profile email";
    /*options.OpenIdConnectEvents.OnTokenValidated = async context =>
    {
       // var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
      //  await userService.CreateUserAsync(context.Principal);
    };*/
});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();


builder.Services.AddFluentUIComponents();
builder.Services.AddOutputCache();
builder.Services.AddHttpContextAccessor();

//add distributed memory cache and configure redis
/*
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions =new ConfigurationOptions{
        EndPoints= { {"redis-12551.c266.us-east-1-3.ec2.redns.redis-cloud.com", 12551} },
        User="default",
        Password="yVYIHSC8phROQ8nyBXMcKum1F23r1ioB",
        AbortOnConnectFail = false
    };
});
*/

// add hybrid cache
#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache(cacheOptions =>
{
    cacheOptions.DefaultEntryOptions = new HybridCacheEntryOptions()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(30),
        Expiration = TimeSpan.FromMinutes(15)
    };
});
#pragma warning restore EXTEXP0018

// register mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<ITooltipService, TooltipService>();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMemoryStorage();
});

builder.Services.AddHangfireServer();

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("FluentMail"));
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();

app.UseHangfireDashboard();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// setup auth endpoints
app.SetupAuthEndpoints();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();