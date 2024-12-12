namespace RR.App.Views;

public partial class EditReceiptPage : ContentPage
{
	public EditReceiptPage(EditReceiptViewModel editReceiptViewModel)
    {
		InitializeComponent();
        BindingContext = editReceiptViewModel;
    }
}