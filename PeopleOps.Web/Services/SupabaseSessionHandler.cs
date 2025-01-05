using System.Text.Json;
using Supabase.Gotrue;
using Blazored.LocalStorage;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
namespace PeopleOps.Web.Services;


public class RedisSessionHandler : Supabase.Gotrue.Interfaces.IGotrueSessionPersistence<Session>
{
    private readonly IDatabase _database;
    private readonly RedisKey _sessionKey = new("authSession");
    public RedisSessionHandler()
    {
        var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints= { {"redis-12551.c266.us-east-1-3.ec2.redns.redis-cloud.com", 12551} },
                User="default",
                Password="yVYIHSC8phROQ8nyBXMcKum1F23r1ioB",
                AbortOnConnectFail = false
            }
        );
        _database = muxer.GetDatabase();
        
        //db.StringSet("foo", "bar");
       // RedisValue result = db.StringGet("foo");
        //var connection = ConnectionMultiplexer.Connect(connectionString);
        //_database = connection.GetDatabase();
    }

    public void SaveSession(Session session)
    {
        var json = JsonSerializer.Serialize(session);
        RedisValue value = new RedisValue(json);
        _database.StringSet(_sessionKey, value);
        Console.WriteLine("Session Saved to Redis");
    }

    public void DestroySession()
    {
        _database.KeyDelete(_sessionKey);
        Console.WriteLine("Session Deleted from Redis");
    }

    public Session? LoadSession()
    {
        Task.Run(() =>
        {
            var json = _database.StringGet(_sessionKey);
            return Task.FromResult(json.HasValue ? JsonSerializer.Deserialize<Session>(json.ToString()) : null);
        }).Wait();
        return null;
    }
    
    public async Task<Session?> LoadSessionAsync()
    {
        var json = await _database.StringGetAsync(_sessionKey);
        return json.HasValue ? JsonSerializer.Deserialize<Session>(json.ToString()) : null;
    }
    
}


public class BrowserSessionHandler : Supabase.Gotrue.Interfaces.IGotrueSessionPersistence<Session>
{
    private readonly ILocalStorageService _localStorageService;
    private const string SessionKey = "supabase_session";

    public BrowserSessionHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async void SaveSession(Session session)
    {
        await _localStorageService.SetItemAsync(SessionKey, session);
        Console.WriteLine("Session Saved");
    }

    public async void DestroySession()
    {
        await _localStorageService.RemoveItemAsync(SessionKey);
        Console.WriteLine("Session Destroyed");
    }

    public Session? LoadSession()
    {
        Task.Run(async () =>
        {
            var session = await _localStorageService.GetItemAsync<Session>(SessionKey);
            return session;
        }).Wait();
        return null;
    }

   
}

public class InMemorySessionHandler : Supabase.Gotrue.Interfaces.IGotrueSessionPersistence<Session>
{
    private Session? _session;
    private readonly IMemoryCache _memoryCache;

    public InMemorySessionHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void SaveSession(Session session)
    {
        _memoryCache.GetOrCreate("session", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return session;
        });
        Console.WriteLine("Session Saved in Memory");
    }

    public void DestroySession()
    {
        _session = new Session();
        _memoryCache.Remove("session");
        Console.WriteLine("Session Destroyed from Memory");
    }

    public Session? LoadSession()
    {
       var result =   _memoryCache.TryGetValue("session", out _session);
        return result ? _session : null;
    }
    
    public Task<Session?> LoadSessionAsync()
    {
        _memoryCache.TryGetValue("session", out _session);
        return Task.FromResult(_session);
    }

}

public class SupabaseSessionHandler : Supabase.Gotrue.Interfaces.IGotrueSessionPersistence<Session>
{
    private readonly ILocalStorageService _localStorageService;
    private const string SessionKey = "supabase_session";
    private const string AccessToken = nameof(AccessToken);
    private const string RefreshToken = nameof(RefreshToken);

    public SupabaseSessionHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async void SaveSession(Session session)
    {
        try
        {
            await _localStorageService.SetItemAsync(SessionKey, session);
            if (session.AccessToken != null) await _localStorageService.SetItemAsync(AccessToken, session.AccessToken);
            if (session.RefreshToken != null) await _localStorageService.SetItemAsync(RefreshToken, session.RefreshToken);
            Console.Write("Session Saved");
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    public async void DestroySession()
    {
        await _localStorageService.RemoveItemAsync(SessionKey);
        Console.Write("Session Destroyed");
    }

    public Session LoadSession()
    {
        var session = _localStorageService.GetItemAsync<Session>(SessionKey).Result;
        return session;
    }
}