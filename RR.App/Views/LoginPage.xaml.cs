using System.Net.Http.Headers;
using System.Net.Http.Json;
using RR.Common.IntermediateModels;

namespace RR.App.Views;

[RegisterAsSingleton]
public partial class LoginPage : ContentPage
{
    public LoginPage(
        LoginViewModel loginViewModel
    )
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
    async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var userInfo = new
        {
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text
        };
        
        var response = await ApiHttpClient.PostAsync("Account/Login", new StringContent(JsonSerializer.Serialize(userInfo), Encoding.UTF8, "application/json")) ??
            throw new Exception("The server did not respond to the login request.");

        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Login Failed", "Invalid username or password", "OK");
            return;
        }
        var json = await response.Content.ReadFromJsonAsync<AuthResponse>();
        ApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json.Token);
        await Shell.Current.GoToAsync($"/{nameof(MainPage)}");
    }
}