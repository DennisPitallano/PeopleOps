using Supabase.Gotrue;
namespace PeopleOps.Web.Services;
using Microsoft.AspNetCore.Http;
public class SupabaseSessionHandler : Supabase.Gotrue.Interfaces.IGotrueSessionPersistence<Session>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SupabaseSessionHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SaveSession(Session session)
    { 
       
        Console.Write("Session Saved");
    }

    public void DestroySession()
    {
        Console.Write("Session Destroyed");
    }

    public Session? LoadSession()
    {
        return new Session();
    }
}