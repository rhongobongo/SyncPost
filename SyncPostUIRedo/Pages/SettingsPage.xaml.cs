using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SyncPostUI.Models;
using SyncPostUI.Services;
using Grid = Microsoft.Maui.Controls.Grid;
using StackLayout = Microsoft.Maui.Controls.StackLayout;

namespace SyncPostUI.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly AuthServices _auth;
    private readonly FacebookServices _fb;

    public SettingsPage(AuthServices authService, FacebookServices facebookServices)
    {
        _auth = authService;
        _fb = facebookServices;
        InitializeComponent();


    }

    private void Label1Tapped(object sender, EventArgs e) //i want to reuse this for hiding and unhiding layouts
    {
        Label label = (Label)sender;

        StackLayout stackLayout = (StackLayout)label.Parent;

        string findLayout = getLayoutName(label.Text);

        StackLayout settingsLayout = stackLayout.FindByName<StackLayout>(findLayout);


        if (settingsLayout.IsVisible)
        {
            settingsLayout.IsVisible = false;
        }
        else
        {
            settingsLayout.IsVisible = true;
        }
    }

    private string? getLayoutName(string LabelName)
    {
        switch (LabelName)
        {
            case "Change Password":
                return "SettingsLayout1";
            case "Connections":
                return "SettingsLayout2";
            default:
                return null;

        }
    }

    private Grid GetButtonGrid(object sender)
    {
        Button button = (Button)sender;

        return (Grid)button.Parent;
    }

    private void btnShowNewPassword(object sender, EventArgs e)
    {
        var parentLayout = GetButtonGrid(sender);

        Entry newPasswordEntry = parentLayout.FindByName<Entry>("NewPassword");

        if (newPasswordEntry != null)
        {
            newPasswordEntry.IsPassword = !newPasswordEntry.IsPassword;
        }
    }

    private void btnShowOldPassword(object sender, EventArgs e)
    {
        var parentLayout = GetButtonGrid(sender);

        Entry oldPasswordEntry = parentLayout.FindByName<Entry>("OldPassword");


        if (oldPasswordEntry != null)
        {
            oldPasswordEntry.IsPassword = !oldPasswordEntry.IsPassword;
        }
    }

    private async void btnChangePassword(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        var parentLayout = (StackLayout)button.Parent;
        Entry newPasswordEntry = parentLayout.FindByName<Entry>("NewPassword");
        Entry oldPasswordEntry = parentLayout.FindByName<Entry>("OldPassword");

        await _auth.ChangePassword(new ChangePWModel(oldPasswordEntry.Text, newPasswordEntry.Text), this);
    }

    private async void btnFacebookClick_Clicked(object sender, EventArgs e)
    {
#if WINDOWS
        WebView _signInWebView = new WebView
        {
            Source = _fb.GetLoginLink(),
            VerticalOptions = LayoutOptions.Fill,
        };
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Row for the WebView
        Grid.SetRow(_signInWebView, 0);
        grid.Children.Add(_signInWebView);

        ContentPage signInContentPage = new ContentPage
        {
            Content = grid,
        };

        try
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(signInContentPage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        _signInWebView.Navigating += async (sender, e) =>
        {

            string code = _fb.OnWebViewNavigating(e, signInContentPage);
            if (e.Url.StartsWith("https://localhost") && code != null)
            {
                string access_token = _fb.ExchangeCodeForAccessToken(code);

                //btnFacebookClick.Text = access_token;
                await SecureStorage.Default.SetAsync("fbauth_token", access_token);
                string oauthToken = await SecureStorage.Default.GetAsync("fbauth_token");
            }

        };
        if(SecureStorage.Default.GetAsync("fbauth_token") != null)
        {
            await showBindedUserName((Button)sender);
        }
#endif
#if ANDROID
        var authResult = await WebAuthenticator.AuthenticateAsync(
        _fb.GetLoginLink(),
        new Uri("https://"));

        var accessToken = authResult?.AccessToken;
#endif
    }


    private async Task showBindedUserName(Button button)
    {
        btnFacebookClick.Text = await _fb.GetAccountConnectedTo(button, this);
    }
    protected override void OnAppearing()
    {
        _ = showBindedUserName(btnFacebookClick);
    }


}
