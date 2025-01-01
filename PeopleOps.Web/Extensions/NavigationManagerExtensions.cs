using Microsoft.AspNetCore.Components;

namespace PeopleOps.Web.Extensions;

public static class NavigationManagerExtensions
{
    public static void NavigateToIgnoreException(this NavigationManager navigationManager, string uri, bool forceLoad = false, bool replace = false)
    {
        try
        {
            navigationManager.NavigateTo(uri, forceLoad, replace);
        }
        catch
        {
            // ignore exception thrown in static rendering within debugger
        }
    }
    
    public static void HttpRedirectTo(this HttpContext httpContext, string redirectionUrl)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.Headers.Append("blazor-enhanced-nav-redirect-location", redirectionUrl);
        httpContext.Response.StatusCode = 200;
    }
}