namespace RR.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CountText))]
    int count = 0;

    public string CountText => Count switch
    {
        0 => "Click me",
        1 => "Clicked 1 time",
        _ => $"Clicked {Count} times"
    };

    public IAsyncRelayCommand CounterButtonClickedCommand =>
        new AsyncRelayCommand(CounterButtonClicked);
    
    async Task CounterButtonClicked()
    {
        await Task.Delay(1000);
        Count++;
    }

    public IAsyncRelayCommand LoginButtonClickedCommand =>
        new AsyncRelayCommand(LoginButtonClicked);
    async Task LoginButtonClicked() =>
        await Shell.Current.GoToAsync(LoginPage.Route);
}
