using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("profiles")]
public class ProfileTable : BaseModel
{
    [PrimaryKey("id")] 
    public int Id { get; set; }

    [Column("username")] 
    public string? UserName { get; set; }

    [Column("updated_at")] 
    public DateTime UpdatedAt { get; set; }

    [Column("avatar_url")] 
    public string? AvatarUrl { get; set; }

    [Column("full_name")] 
    public string? FullName { get; set; }

    [Column("first_name")] 
    public string? FirstName { get; set; }

    [Column("last_name")] 
    public string? LastName { get; set; }

    [Column("date_of_birth")] 
    public DateTime? DateOfBirth { get; init; }

    [Column("gender")]
    public bool? Gender { get; set; }

    [Column("email")] 
    public string? Email { get; set; }

    [Column("job_title")] 
    public string? JobTitle { get; set; }

    [Column("city_address")]
    public string? CityAddress { get; init; }
    [Column("auth0_user_id")] 
    public string? Auth0UserId { get; set; }
}