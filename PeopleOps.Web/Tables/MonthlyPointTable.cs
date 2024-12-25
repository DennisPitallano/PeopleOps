using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("monthly_points")]
public class MonthlyPointTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("profile_id")]
    public long ProfileId { get; set; }
    
    [Column("points_allocated")]
    public int PointsAllocated { get; set; }
    
    [Column("points_spent")]
    public int PointsSpent { get; set; }
    
    [Column("month_year")]
    public DateTime Month { get; set; }
}