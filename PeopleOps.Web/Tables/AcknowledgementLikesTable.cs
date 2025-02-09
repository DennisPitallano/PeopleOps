using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("acknowledgement_likes")]
public class AcknowledgementLikesTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    [Column("acknowledgement_id")]
    public long AcknowledgementId { get; set; }
    [Column("liker_id")]
    public int LikerId { get; set; }
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}