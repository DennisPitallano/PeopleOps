using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("profiles")]
public class ProfileTable: BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string LastName { get; set; }
    
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }
    
    [Column("gender")]
    public bool Gender { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
}