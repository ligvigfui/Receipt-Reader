namespace RR.Data.DataBaseObjects;

[Tables(nameof(AddressDBO))]
public class AddressDBO : AbstractPublicOwnable
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
    public AddressDBO() { }
    public AddressDBO(Address address, int userShortId)
    {
        UserShortId = userShortId;
        GroupId = address.GroupId;
        if (address.Id is not null)
            Id = address.Id.Value;
        Country = address.Country;
        Region = address.Region;
        PostalCode = address.PostalCode;
        City = address.City;
        StreetAddress = address.StreetAddress;
        Note = address.Note;
    }

    public static implicit operator Address?(AddressDBO? addressDBO) => addressDBO is null ? null : new()
    {
        Id = addressDBO.Id,
        GroupId = addressDBO.GroupId,
        Country = addressDBO.Country,
        Region = addressDBO.Region,
        PostalCode = addressDBO.PostalCode,
        City = addressDBO.City,
        StreetAddress = addressDBO.StreetAddress,
        Note = addressDBO.Note
    };
}