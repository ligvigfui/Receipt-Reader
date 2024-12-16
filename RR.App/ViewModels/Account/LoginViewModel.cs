using Microsoft.Maui.ApplicationModel.Communication;
namespace RR.App.ViewModels;

public partial class LoginViewModel(IRRApiService RRApiService) : ObservableObject
{
    [ObservableProperty]
    string email = string.Empty;
    [ObservableProperty]
    string password = string.Empty;

    public IAsyncRelayCommand LoginButtonClicked2Command =>
        new AsyncRelayCommand(LoginButtonClicked2);

    async Task LoginButtonClicked2()
    {
        try
        {
            Login userInfo = new()
            {
                Email = Email,
                Password = Password,
            };
            await RRApiService.Login(userInfo);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Login failed", ex.Message, "cancel");
        }
    }
}
