using SyncPostUI.Models;
using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class HistoryPage : ContentPage
{
    private readonly TemplateServices _templateService;
    private readonly AuthServices _authServices;
    public HistoryPage(TemplateServices templateService, AuthServices authServices)
    {
		InitializeComponent();
        _templateService = templateService;
        _authServices = authServices;
        sortPicker.SelectedIndex = 0;
        OnAppearing();

	}
    private async void LoadTemplates()
    {
        var templates = await _templateService.GetHistories(this);
        if (templates != null)
        {
            switch (sortPicker.SelectedIndex)
            {
                case 0: // Sort by Time Modified
                    templates = reverseSwitch.IsToggled ? templates.OrderByDescending(t => t.PostDate).ToList() : templates.OrderBy(t => t.PostDate).ToList();
                    break;
                case 1: // Sort Alphabetically
                    templates = reverseSwitch.IsToggled ? templates.OrderByDescending(t => t.PostContent).ToList() : templates.OrderBy(t => t.PostContent).ToList();
                    break;
            }
            TemplatesListView.BindingContext = new HistoryViewModel { Templates = templates };
        }
    }
    public class HistoryViewModel
    {
        public List<PostHistory> Templates { get; set; }
    }
    protected override void OnAppearing()
    {
        LoadTemplates();
    }
    private async void OnTemplateTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
             return;


        var selectedTemplate = (PostHistory)e.Item;
        //await Navigation.PushAsync(new TemplateExistingEditorPage(_templateService, _authServices, selectedTemplate));
        await DisplayAlert("Details",
            $"Content: \n{selectedTemplate.PostContent}\n" +
            $"Date: {selectedTemplate.PostDate}\n" +
            $"Posted on: {string.Join("\n", selectedTemplate.PostedTo)}", // Join list elements with comma and space
            "Close");

    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = searchEntry.Text.ToLower();
        var filteredTemplates = (await _templateService.GetHistories(this)).Where(t =>
            t.PostContent.ToLower().Contains(searchText) ||
            t.PostedTo.Any(pt => pt.ToLower().Contains(searchText))
        ).ToList();
        TemplatesListView.BindingContext = new HistoryViewModel { Templates = filteredTemplates };
    }

    private async void OnSortPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTemplates();
    }

    private async void OnReverseToggled(object sender, ToggledEventArgs e)
    {
        LoadTemplates();
    }





}