using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class DailyQuestTableResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("quest_id")]
    public long QuestId { get; set; }
    
    [JsonPropertyName("accepted_date")]
    public DateTime AcceptedDate { get; set; }
    
    [JsonPropertyName("completion_status")]
    public bool CompletionStatus { get; set; }
    
    [JsonPropertyName("completion_date")]
    public DateTime? CompletionDate { get; set; }
    
    [JsonPropertyName("quest_group")]
    public string QuestGroup { get; set; }
    
    [JsonPropertyName("quest_name")]
    public string QuestName { get; set; }
    
    [JsonPropertyName("quest_description")]
    public string QuestDescription { get; set; }
    
    [JsonPropertyName("points")]
    public int QuestPoints { get; set; }
    
    [JsonPropertyName("quest_type")]
    public bool IsMainQuest { get; set; }
}