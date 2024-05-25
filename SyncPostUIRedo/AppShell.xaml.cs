using SyncPostUI.Pages;


namespace SyncPostUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(LogoutPage), typeof(LogoutPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(TemplatesListPage), typeof(TemplatesListPage));
            Routing.RegisterRoute(nameof(TemplateNewEditorPage), typeof(TemplateNewEditorPage));
            Routing.RegisterRoute(nameof(TemplateExistingEditorPage), typeof(TemplateExistingEditorPage));
            Routing.RegisterRoute(nameof(PostingPage), typeof(PostingPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
        }
    }
}
