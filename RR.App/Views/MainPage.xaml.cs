namespace RR.App.Views;

[RegisterAsSingleton]
public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    async void OnLoginButtonClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync($"/{nameof(LoginPage)}");
}
