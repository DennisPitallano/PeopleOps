using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class TagResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }
}