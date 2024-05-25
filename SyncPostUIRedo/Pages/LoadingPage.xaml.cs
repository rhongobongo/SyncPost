using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class LoadingPage : ContentPage
{
    private readonly AuthServices _authService;
    public LoadingPage(AuthServices authService)
    {

        InitializeComponent();
        _authService = authService;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (await _authService.PersistentLogin(this) == true)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}