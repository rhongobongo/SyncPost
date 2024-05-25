using SyncPostUI.Models;
using SyncPostUI.Services;
using System.Collections.ObjectModel;

namespace SyncPostUI.Pages;

public partial class TemplateExistingEditorPage : ContentPage
{
    public ObservableCollection<PageModel> Pages { get; set; }
    private readonly TemplateServices _templateService;
    private readonly AuthServices _authServices;
    private readonly TemplateModel templateModel1;
    public TemplateExistingEditorPage(TemplateServices templateService, AuthServices authServices, TemplateModel templateModel)
    {
        InitializeComponent();
        _templateService = templateService;
        _authServices = authServices;
        editorTemplateContent.Text = templateModel.Content;
        entryTemplateTitle.Text = templateModel.Title;
        templateModel1 = templateModel;
        OnAppearing();
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        await _templateService.SaveChangesTemplate(new ModifyTemplateModel { templateID = templateModel1.TemplateID, title = entryTemplateTitle.Text, content = editorTemplateContent.Text, template_tags = entryTags.Text }, this);
    }

    private async void btnNewSave_Clicked(object sender, EventArgs e)
    {
        await _templateService.SaveNewTemplate(new Models.CreateTemplateModel { template_title = entryTemplateTitle.Text, template_content = editorTemplateContent.Text, template_tags = entryTags.Text }, this);
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        await _templateService.DeleteTemplate(new DeleteTemplateModel { templateID = templateModel1.TemplateID }, this);
        await Shell.Current.GoToAsync($"//{nameof(TemplatesListPage)}");
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(TemplatesListPage)}");
    }

    private async void btnPostingPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PostingPage(_templateService, _authServices, new APITemplateModel(){ Content = editorTemplateContent.Text}, new FacebookServices()));
    }

    protected override void OnAppearing()
    {
        if(templateModel1.template_tags != null)
        {
            entryTags.Text = string.Join(",", templateModel1.template_tags);
        }
    }



}