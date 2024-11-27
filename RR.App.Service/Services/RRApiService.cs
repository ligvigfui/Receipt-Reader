namespace RR.App.Service.Services;

public class RRApiService(IHttpClientFactory httpClientFactory) : IRRApiService
{
    readonly HttpClient ApiHttpClient = httpClientFactory.CreateClient(HttpClientConstants.RRApiClient);

    void HandleTokenRefreshIsProvided(HttpResponseMessage response)
    {
        //get the Response.Headers.Authorization
        var token = response.Headers.GetValues("Authorization");
        if (token == null || !token.Any())
            return;

        ApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationScheme.Bearer.ToString(), token.FirstOrDefault());
    }

    public async Task Login(Login loginModel)
    {
        var response = await ApiHttpClient.PostAsync("Account/Login", new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json")) ??
            throw new Exception("The server did not respond to the login request.");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<AuthResponse>();
        ApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationScheme.Bearer.ToString(), json?.Token);
    }
}
