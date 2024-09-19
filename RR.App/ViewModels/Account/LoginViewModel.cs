namespace RR.App.ViewModels;

public partial class LoginViewModel(IHttpClientFactory httpClientFactory)
{
    HttpClient ApiHttpClient { get; set; } = httpClientFactory.CreateClient(HttpClientConstants.ApiClient);

}
