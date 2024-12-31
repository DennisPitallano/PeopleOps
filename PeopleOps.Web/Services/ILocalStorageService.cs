namespace PeopleOps.Web.Services;

public interface ILocalStorageServices
{
    Task SetItemAsync(string key, string value);
    Task SetItemAsync<T>(string key, T value);
    Task<string> GetItemAsync(string key);
    Task<T> GetItemAsync<T>(string key) where T : new();
    Task RemoveItemAsync(string key);
    Task ClearAsync();
}