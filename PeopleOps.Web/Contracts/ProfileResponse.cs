namespace PeopleOps.Web.Contracts;

public class ProfileResponse
{
    public Guid Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    public string Email { get; set; }

    public string? JobTitle { get; set; }

    public string? CityAddress { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsProfileIncomplete =>
        // check job title, city address, date of birth and gender if they are null
        string.IsNullOrEmpty(JobTitle) 
        || string.IsNullOrEmpty(CityAddress) 
        || !DateOfBirth.HasValue 
        || !Gender.HasValue;
}