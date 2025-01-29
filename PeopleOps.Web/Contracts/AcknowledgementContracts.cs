using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class AcknowledgementRequest
{
    [Required (ErrorMessage = "Sender is required")]
    public int SenderId { get; set; }
    [Required (ErrorMessage = "Receiver is required")]
    public int ReceiverId { get; set; }
    
    [Required (ErrorMessage = "Receiver list is required")]
    // should be greater than 0
    [MinLength(1, ErrorMessage = "Receiver list should be greater than 0")]
    public List<int> ReceiverList { get; set; } = new();
    public DateTime AcknowledgmentDate { get; set; }
    [Required (ErrorMessage = "Message is required")]
    public string Message { get; set; }
    
    [Required (ErrorMessage = "Tags are required")]
    // should be greater than 0
    [MinLength(1, ErrorMessage = "Tags should be greater than 0")]
    public List<long> TagIds { get; set; } = new();
    
    
    [Required (ErrorMessage = "Coins are required")]
    // should be greater than 0
    [Range(1, 100, ErrorMessage = "Coins should be greater than 0")]
    public int Coins { get; set; }
    
    public List<TagResponse> AcknowledgementTags { get; set; } = new();
}

public class AcknowledgementResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("sender_id")]
    public int SenderId { get; set; }
    [JsonPropertyName("acknowledgment_date")]
    public DateTime AcknowledgmentDate { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
}

public class AcknowledgementTagResponse
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long AcknowledgmentId { get; set; }
    public long TagId { get; set; }
}

public class AcknowledgementTagRequest
{
    public long AcknowledgmentId { get; set; }
    public long TagId { get; set; }
}