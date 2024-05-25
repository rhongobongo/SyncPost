using SyncPostUI.Models;
using SyncPostUI.Services;

namespace SyncPostUI.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly AuthServices _auth;
        private readonly TemplateServices _template;
        public MainPage(AuthServices _AuthService, TemplateServices templateServices)
        {
            InitializeComponent();
            _auth = _AuthService;
            _template = templateServices;
            OnAppearing();
        }

        protected override void OnAppearing()
        {
            InitializeAsync();
        }


        private async void InitializeAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(TemplatesListPage)}");
        }

    }
}
