using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class LoginPage : ContentPage
{
   private readonly AuthServices _authServices;
    public LoginPage(AuthServices authServices)
    {
        InitializeComponent();
        _authServices = authServices;
        UsernameEntry.Text = "";
        PasswordEntry.Text = "";
        UsernameEntry.Placeholder = _authServices.Token();
    }

    private async void btnOnLogin(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        try
        {
            AuthServices authService = new AuthServices();
            bool isAuthenticated = await authService.LoginAsync(username, password, this);

            if (isAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await DisplayAlert("Error", "Authentication failed", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


    private void btnShowPass(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }

    private async void btnRegistrationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
    }

}