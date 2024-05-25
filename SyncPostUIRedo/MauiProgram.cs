using Microsoft.Extensions.Logging;
using SyncPostUI.Constants;
using SyncPostUI.Pages;
using SyncPostUI.Services;

namespace SyncPostUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            #region Transient

            #region Pages
            builder.Services.AddTransient<AuthServices>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LogoutPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<RegistrationPage>();

            builder.Services.AddTransient<TemplatesListPage>();
            builder.Services.AddTransient<TemplateNewEditorPage>();
            builder.Services.AddTransient<TemplateExistingEditorPage>();
            builder.Services.AddTransient<PostingPage>();
            builder.Services.AddTransient<HistoryPage>();
            #endregion

            #region Services
            builder.Services.AddTransient<MiscServices>();
            builder.Services.AddTransient<TemplateServices>();
            builder.Services.AddTransient<FacebookServices>();
            #endregion

            #endregion

            builder.Services.AddHttpClient(AppConstants.HttpClientName, httpClient =>
            {
                var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7150" : "https://localhost:7150";
                httpClient.BaseAddress = new Uri(baseAddress);
            }); 
            return builder.Build();
        }
    }
}
