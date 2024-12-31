using System.Text.Json;

namespace PeopleOps.Web.Services;

public class LocalStorageService : ILocalStorageServices
{
    private readonly IJSRuntime _jsRuntime;

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    // set for object type value in local storage
    public async Task SetItemAsync<T>(string key, T value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem ", key, JsonSerializer.Serialize(value));
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    // get for object type value in local storage
    public async Task<T> GetItemAsync<T>(string key) where T : new()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return (string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json)) ?? new T();
    }

    public async Task RemoveItemAsync(string key)
    {
        if (_jsRuntime is IJSInProcessRuntime jsInProcessRuntime)
        {
            await jsInProcessRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }

    public async Task ClearAsync()
    {
        if (_jsRuntime is IJSInProcessRuntime jsInProcessRuntime)
        {
            await jsInProcessRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
}