namespace RR.App.Views;

public partial class LoginPage : ContentPage
{
    public static string Route => "Login";
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}