namespace RR.App.Views;

[RegisterAsSingleton]
public partial class LoginPage : ContentPage
{
    HttpClient ApiHttpClient { get; set; }
    public LoginPage(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();
        ApiHttpClient = httpClientFactory.CreateClient(HttpClientConstants.ApiClient);
    }
    async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var userInfo = new
        {
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text
        };
        
        var response = await ApiHttpClient.PostAsync("Account/Register", new StringContent(JsonSerializer.Serialize(userInfo), Encoding.UTF8, "application/json")) ??
            throw new Exception("The server did not respond to the login request.");

        if (response.IsSuccessStatusCode)
        {
            await Shell.Current.GoToAsync(nameof(MainPage));
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password", "OK");
        }
    }
}