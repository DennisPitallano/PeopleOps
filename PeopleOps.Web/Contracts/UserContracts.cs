using System.Text.Json.Serialization;
using Supabase.Functions.Responses;

namespace PeopleOps.Web.Contracts;

public class UserRequest
{
    
}

public class UserResponse : BaseResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }
    
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    
    [JsonPropertyName("gender")]
    public bool? Gender { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("job_title")]
    public string? JobTitle { get; set; }
    
    [JsonPropertyName("city_address")]
    public string? CityAddress { get; set; }

    public string Initials => $"{FirstName?[0]}{LastName?[0]}";

    public string GetAvatar => AvatarUrl ?? $"https://ui-avatars.com/api/?name={FirstName}+{LastName}&background=random&color=fff&size=128";
}