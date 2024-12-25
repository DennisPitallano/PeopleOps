using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("profile_quests")]
public class ProfileQuestTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("profile_id")]
    public long ProfileId { get; set; }
    
    [Column("quest_id")]
    public long QuestId { get; set; }
    
    [Column("accepted_date")]
    public DateTime AcceptedDate { get; set; }
    
    [Column("completion_status")]
    public bool CompletionStatus { get; set; }
    
    [Column("completion_date")]
    public DateTime CompletionDate { get; set; }
}