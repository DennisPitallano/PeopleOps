using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("acknowledgments")]
public class AcknowledgmentTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("sender_id")]
    public long SenderId { get; set; }
    
    [Column("receiver_id")]
    public long ReceiverId { get; set; }
    
    [Column("acknowledgment_date")]
    public DateTime AcknowledgmentDate { get; set; }
    
    [Column("message")]
    public string Message { get; set; }
    
}