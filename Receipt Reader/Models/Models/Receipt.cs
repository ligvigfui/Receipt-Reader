namespace RR.Common.Models;

[Table(nameof(Receipt))]
public class Receipt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; } = null;
    int VendorId { get; set; }
    [JsonIgnore]
    public virtual Vendor Vendor { get; set; }
    string ItemsJson
    {
        get => JsonSerializer.Serialize(Items);
        set => Items = JsonSerializer.Deserialize<List<ReceiptItem>>(value) ?? [];
    }
    [JsonIgnore]
    public virtual List<ReceiptItem> Items { get; set; } = [];
    public long Total => Items.Sum(i => i.Price);
    public string ReceiptId { get; set; }
    public DateTime DateTime { get; set; }
}
