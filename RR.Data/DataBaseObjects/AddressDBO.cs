namespace RR.Data.DataBaseObjects;

[Tables(nameof(AddressDBO))]
public class AddressDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public string? Note { get; set; }

    public static implicit operator Address(AddressDBO addressDBO) => new()
    {
        Country = addressDBO.Country,
        Region = addressDBO.Region,
        PostalCode = addressDBO.PostalCode,
        City = addressDBO.City,
        StreetAddress = addressDBO.StreetAddress,
        Note = addressDBO.Note
    };

    public static implicit operator AddressDBO(Address address) => new()
    {
        Country = address.Country,
        Region = address.Region,
        PostalCode = address.PostalCode,
        City = address.City,
        StreetAddress = address.StreetAddress,
        Note = address.Note
    };
}