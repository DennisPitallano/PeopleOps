using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PeopleOps.Web.Contracts;

public class ProfileRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    // must be 15 years from now
    [Range(typeof(DateTime), "1/1/1900", "1/1/2010", ErrorMessage = "Date of birth must be 15 years from now")]
    public DateTime? DateOfBirth { get; set; }

    public bool Gender { get; set; } = true;

    [Required(ErrorMessage = "Job title is required")]
    public string? JobTitle { get; set; }

    [Required(ErrorMessage = "City address is required")]
    public string? CityAddress { get; set; }
}

[RequiresUnreferencedCode("Necessary because of RangeAttribute usage")]
public class ProfileResponse
{
    public Guid Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    // must be 15 years from now
    [Range(typeof(DateTime), "1/1/1900", "1/1/2010", ErrorMessage = "Date of birth must be 15 years from now")]
    public DateTime? DateOfBirth { get; set; }

    public bool? Gender { get; set; } = true;

    public string Email { get; set; }

    [Required(ErrorMessage = "Job title is required")]
    public string? JobTitle { get; set; }

    [Required(ErrorMessage = "City address is required")]
    public string? CityAddress { get; set; }

    public string? AvatarUrl { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsProfileIncomplete =>
        // check job title, city address, date of birth and gender if they are null
        string.IsNullOrEmpty(JobTitle)
        || string.IsNullOrEmpty(CityAddress)
        || !DateOfBirth.HasValue
        || !Gender.HasValue;

    public string GetAvatar =>
        AvatarUrl ?? $"https://ui-avatars.com/api/?name={FirstName}+{LastName}&background=random&color=fff&size=128";
}