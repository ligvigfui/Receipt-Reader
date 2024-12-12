namespace RR.App.Views;

public partial class AddressPage : ContentPage, IRoute
{
    public static string Route => "Address";
    public AddressPage(AddressViewModel addressViewModel)
    {
		InitializeComponent();
        BindingContext = addressViewModel;
    }
}