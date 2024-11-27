namespace RR.App.Views;

public partial class LoginPage : ContentPage, IRoute
{
    public static string Route => "Login";
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}