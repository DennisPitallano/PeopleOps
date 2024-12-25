using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("quests")]
public class QuestTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("quest_type")]
    public bool QuestType { get; set; }
    
    [Column("quest_name")]
    public string QuestName { get; set; }
    
    [Column("quest_description")]
    public string QuestDescription { get; set; }
    
    [Column("points")]
    public int Points { get; set; }
}