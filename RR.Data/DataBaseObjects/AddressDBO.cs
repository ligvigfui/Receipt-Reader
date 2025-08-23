namespace RR.Data.DataBaseObjects;

[Tables(nameof(AddressDBO))]
public class AddressDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string PostalCode { get; set; }
    public string? Country { get; set; }
    public string City { get; set; }
    public string? Region { get; set; }
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string Number { get; set; }
    public string? Appratment { get; set; }
    public string? Note { get; set; }

    public static implicit operator Address(AddressDBO addressDBO) => new()
    {
        PostalCode = addressDBO.PostalCode,
        Country = addressDBO.Country,
        City = addressDBO.City,
        Region = addressDBO.Region,
        Line1 = addressDBO.Line1,
        Line2 = addressDBO.Line2,
        Number = addressDBO.Number,
        Apartment = addressDBO.Appratment,
        Note = addressDBO.Note
    };

    public static implicit operator AddressDBO(Address address) => new()
    {
        PostalCode = address.PostalCode,
        Country = address.Country,
        City = address.City,
        Region = address.Region,
        Line1 = address.Line1,
        Line2 = address.Line2,
        Number = address.Number,
        Appratment = address.Apartment,
        Note = address.Note
    };
}