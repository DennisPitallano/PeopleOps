using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("users")]
public class ProfileTable: BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("user_name")]
    public string UserName { get; set; }

    [Column("updated_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("full_name")]
    public string FullName { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string LastName { get; set; }
    
    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }
    
    [Column("gender")]
    public bool? Gender { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("job_title")]
    public string? JobTitle { get; set; }
    
    [Column("city_address")]
    public string? CityAddress { get; set; }
    
}