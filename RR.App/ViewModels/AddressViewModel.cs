namespace RR.App.ViewModels;

public partial class AddressViewModel : ObservableObject
{
    [ObservableProperty]
    string? postalCode;
    [ObservableProperty]
    string? country;
    [ObservableProperty]
    string? city;
    [ObservableProperty]
    string? region;
    [ObservableProperty]
    string? line1;
    [ObservableProperty]
    string? line2;
    [ObservableProperty]
    string? number;
    [ObservableProperty]
    string? apartment;
    [ObservableProperty]
    string? note;
}
