using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("attendance")]
public class AttendanceTable : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    [Column("profile_id")]
    public int ProfileId { get; set; }
    
    [Column("login_time")]
    public DateTime? TimeIn { get; set; } 
    
    [Column("logout_time")]    
    public DateTime? TimeOut { get; set; }
    
    [Column("activity_date")]
    public DateOnly ActivityDate { get; set; }
}