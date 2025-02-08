using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("monthly_points")]
public class MonthlyPointsTable : BaseModel
{
    [PrimaryKey("id")] 
    public long Id { get; set; }

    [Column("created_at")] 
    public DateTime CreatedAt { get; set; }

    [Column("month_year")] 
    public DateOnly MonthYear { get; set; }
    
    [Column("points_allocated")] 
    public int PointsAllocated { get; set; }
    
    [Column("points_spent")] 
    public int PointsSpent { get; set; }

    [Column("profile_id")] 
    public int ProfileId { get; set; }
    
    [Column("is_revoke")]
    public bool IsRevoke { get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("month")]
    public int Month { get; set; }
}