namespace RR.App.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    void RegisterRoutes()
    {
        Routing.RegisterRoute(LoginPage.Route, typeof(LoginPage));
    }
}
