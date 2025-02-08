using System.Text.Json.Serialization;

namespace PeopleOps.Web.Contracts;

public class MonthlyPointsResponse
{
    [JsonPropertyName("id")] 
    public long Id { get; set; }
    [JsonPropertyName("created_at")] 
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("month_year")]
    public DateOnly MonthYear { get; set; }
    [JsonPropertyName("points_allocated")]
    public int PointsAllocated { get; set; }
    [JsonPropertyName("points_spent")]
    public int PointsSpent { get; set; }
    [JsonPropertyName("profile_id")]
    public int ProfileId { get; set; }
    [JsonPropertyName("is_revoke")]
    public bool IsRevoke { get; set; }
    [JsonPropertyName("year")]
    public int Year { get; set; }
    [JsonPropertyName("month")]
    public int Month { get; set; }
    public int AvailableCoins => PointsAllocated - PointsSpent;

    public string AvailableCoinsLabel => AvailableCoins > 0
        ? $"You have {AvailableCoins} coins available to give."
        : "You have no coins available";
}