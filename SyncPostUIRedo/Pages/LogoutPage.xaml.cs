using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class LogoutPage : ContentPage
{
    private readonly AuthServices auth;
    public LogoutPage(AuthServices authService)
    {
        InitializeComponent();
        auth = authService;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LogoutAction();
    }

    private async void LogoutAction()
    {
        auth.Logout();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        await Shell.Current.Navigation.PopToRootAsync();
    }
}