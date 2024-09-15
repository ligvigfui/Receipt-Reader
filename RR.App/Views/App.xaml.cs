namespace RR.App.Views;

[RegisterAsSingleton]
public partial class App : Application
{

    public App(AppShell appShell)
    {
        InitializeComponent();
        
        MainPage = appShell;
    }
}
