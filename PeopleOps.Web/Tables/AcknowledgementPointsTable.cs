using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("acknowledgment_points")]
public class AcknowledgementPointsTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("acknowledgment_id")]
    public long AcknowledgmentId { get; set; }
    
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }
    
    [Column("points_earned")]
    public int Coins { get; set; }
}