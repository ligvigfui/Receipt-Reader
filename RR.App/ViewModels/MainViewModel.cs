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

    [RelayCommand]
    void CounterButtonClicked()
    {
        Count++;
    }

    [RelayCommand]
    async Task LoginButtonClicked() =>
        await Shell.Current.GoToAsync($"{LoginPage.Route}");
}
