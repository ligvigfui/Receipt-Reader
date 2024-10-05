namespace RR.App.ViewModels;

public partial class LoginViewModel(IHttpClientFactory httpClientFactory) : ObservableObject
{
    readonly HttpClient ApiHttpClient = httpClientFactory.CreateClient(HttpClientConstants.ApiClient);

    [ObservableProperty]
    string email = string.Empty;
    [ObservableProperty]
    string password = string.Empty;

    [RelayCommand]
    async Task LoginButtonClicked()
    {

        var userInfo = new
        {
            Email,
            Password
        };

        var response = await ApiHttpClient.PostAsync("Account/Login", new StringContent(JsonSerializer.Serialize(userInfo), Encoding.UTF8, "application/json")) ??
            throw new Exception("The server did not respond to the login request.");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<AuthResponse>();
        ApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json.Token);
        await Shell.Current.GoToAsync($"..");
    }
}
