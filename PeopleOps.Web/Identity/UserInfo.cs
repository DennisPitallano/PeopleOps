namespace PeopleOps.Web.Identity;

public class UserInfo
{
    public required string? UserId { get; set; }
    public required string? Email { get; set; }
    public string? AvatarUrl { get; set; }
    public string? FullName { get; set; }
    public string? GivenName { get; set; }
    public string? SurName { get; set; }
}