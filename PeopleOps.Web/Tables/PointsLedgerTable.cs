using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PeopleOps.Web.Tables;

[Table("points_ledger")]
public class PointsLedgerTable : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("profile_id")]
    public long ProfileId { get; set; }
    
    [Column("transaction_date")]
    public DateTime TransactionDate { get; set; }
    
    [Column("transaction_type")]
    public string TransactionType { get; set; }
    
    [Column("points")]
    public int Points { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("reference_id")]
    public long ReferenceId { get; set; }
    
    [Column("reference_table")]
    public string ReferenceTable { get; set; }
    
}