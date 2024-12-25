using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("attendance")]
public class AttendanceTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("profile_id")]
    public long ProfileId { get; set; }
    
    [Column("time_in")]
    public DateTime? TimeIn { get; set; }
    
    [Column("time_out")]    
    public DateTime? TimeOut { get; set; }
    
    [Column("activity_date")]
    public DateTime ActivityDate { get; set; }
}