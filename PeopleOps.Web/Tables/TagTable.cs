
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("tags")]
public class TagTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("tag_name")]
    public string? TagName { get; set; }
}