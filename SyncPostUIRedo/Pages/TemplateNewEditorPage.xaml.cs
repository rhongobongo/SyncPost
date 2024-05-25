using Microsoft.Maui.Controls;
using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class TemplateNewEditorPage : ContentPage
{
    private readonly TemplateServices _templateService;
    private readonly AuthServices _authServices;
    public TemplateNewEditorPage(TemplateServices templateService, AuthServices authServices)
    {
        InitializeComponent();
        _templateService = templateService;
        _authServices = authServices;
    }

    private async void btnNewSave_Clicked(object sender, EventArgs e)
    {
       await _templateService.SaveNewTemplate(new Models.CreateTemplateModel { template_title = entryTemplateTitle.Text, template_content = editorTemplateContent.Text }, this);
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(TemplatesListPage)}");
    }

    private async void btnPostingPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PostingPage(_templateService, _authServices, new Models.APITemplateModel {Content = editorTemplateContent.Text}, new FacebookServices()));
    }
}