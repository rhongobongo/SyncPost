using SyncPostUI.Models;
using SyncPostUI.Services;
using System.Text;
using System.Text.Json;

namespace SyncPostUI.Pages;

public partial class RegistrationPage : ContentPage
{
    private readonly AuthServices auth;
    public RegistrationPage(AuthServices authService)
    {
        InitializeComponent();
        auth = authService;
    }

    private void btnShowPass(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }

    private async void btnRegister(object sender, EventArgs e)
    {
        var userAccount = new RegistrationModel(UsernameEntry.Text, PasswordEntry.Text);


        try
        {
            bool isConfirmed = await auth.Register(userAccount);
            if(isConfirmed == true)
            {
                await DisplayAlert("Success", "Registration Success", "Exit");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert($"Failed to register user. Status code: {ex}", " ", " ");
        }
    }
    private async void btnLoginPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}