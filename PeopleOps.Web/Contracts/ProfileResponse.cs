namespace PeopleOps.Web.Contracts;

public class ProfileResponse
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public bool Gender { get; set; }
    
    public string Email { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}