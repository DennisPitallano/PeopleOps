using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("acknowledgement_tags")]
public class AcknowledgementTagTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("acknowledgment_id")]
    public long AcknowledgmentId { get; set; }
    
    [Column("tag_id")]
    public long TagId { get; set; }
    
}