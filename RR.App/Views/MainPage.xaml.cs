namespace RR.App.Views;

public partial class MainPage : ContentPage, IRoute
{
    public static string Route => "Main";
    public MainPage(MainViewModel mainViewModel)
    {
        InitializeComponent();
        BindingContext = mainViewModel;
    }
}
