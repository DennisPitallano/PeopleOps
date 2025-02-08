using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class ProfileRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    // must be 15 years from now
    [Range(typeof(DateTime), "1/1/1900", "1/1/2010", ErrorMessage = "Date of birth must be 15 years from now")]
    public DateTime? DateOfBirth { get; set; }

    public bool Gender { get; set; } = true;

    [Required(ErrorMessage = "Job title is required")]
    public string? JobTitle { get; set; }

    [Required(ErrorMessage = "City address is required")]
    public string? CityAddress { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    [Required(ErrorMessage = "Auth0 user id is required")]
    public string? Auth0UserId { get; set; }
    public string? UserName { get; set; }
}

[RequiresUnreferencedCode("Necessary because of RangeAttribute usage")]
public class ProfileResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
    
    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }
    
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    
    [JsonPropertyName("gender")]
    public bool? Gender { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("job_title")]
    public string? JobTitle { get; set; }
    
    [JsonPropertyName("city_address")]
    public string? CityAddress { get; set; }
    [JsonPropertyName("auth0_user_id")]
    public string? Auth0UserId { get; set; }

    public string Initials => $"{FirstName?[0]}{LastName?[0]}";

    public bool IsProfileIncomplete =>
        // check job title, city address, date of birth and gender if they are null
        string.IsNullOrEmpty(JobTitle)
        || string.IsNullOrEmpty(CityAddress)
        || !DateOfBirth.HasValue
        || !Gender.HasValue;

    public string GetAvatar =>
        AvatarUrl ?? $"https://ui-avatars.com/api/?name={FirstName}+{LastName}&background=random&color=fff&size=128";
}
