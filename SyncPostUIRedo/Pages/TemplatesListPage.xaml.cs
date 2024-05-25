using SyncPostUI.Models;
using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class TemplatesListPage : ContentPage
{
    private readonly TemplateServices _templateService;
    private readonly AuthServices _authServices;
    public TemplatesListPage(TemplateServices templateService, AuthServices authServices)
    {
		InitializeComponent();
        _templateService = templateService;
        _authServices = authServices;
        searchEntry.Text = string.Empty;
        OnAppearing();

	}
    private async void LoadTemplates()
    {
        var templates = await _templateService.GetTemplates(this);
        if (templates != null)
        {
            
            // Get the search text entered by the user
            string searchText = searchEntry.Text.ToLower();

            // Filter templates based on the selected search criteria
            switch (searchPicker.SelectedIndex)
            {
                case 0: // Search by Name
                    templates = templates.Where(t => t.Title.ToLower().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;
                case 1: // Search by Tags
                    templates = templates.Where(t => t.template_tags != null && t.template_tags.Any(tag => tag.ToLower().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    break;
                case 2: // Search by Content
                    templates = templates.Where(t => t.Content.ToLower().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;

            }

            // Sort templates based on the selected sorting criteria
            if (sortPicker.SelectedIndex == 0) // Assuming 0 corresponds to "Chronologically"
            {
                if (reverseSwitch.IsToggled)
                {
                    templates = templates.OrderByDescending(t => t.last_modification_date).ToList();
                }
                else
                {
                    templates = templates.OrderBy(t => t.last_modification_date).ToList();
                }
            }
            else if (sortPicker.SelectedIndex == 1) // Assuming 1 corresponds to "Alphabetically"
            {
                if (reverseSwitch.IsToggled)
                {
                    templates = templates.OrderBy(t => t.Title).Reverse().ToList();
                }
                else
                {
                    templates = templates.OrderBy(t => t.Title).ToList();
                }
            }

            TemplatesListView.BindingContext = new TemplatesListViewModel { Templates = templates };
        }
    }


    private void OnReverseToggled(object sender, ToggledEventArgs e)
    {
        LoadTemplates();
    }
    public class TemplatesListViewModel
    {
        public List<TemplateModel> Templates { get; set; }
    }
    protected override void OnAppearing()
    {
        sortPicker.SelectedIndex = 0;
        searchPicker.SelectedIndex = 0;
        LoadTemplates();

    }
    private async void OnTemplateTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
            return;


        var selectedTemplate = (TemplateModel)e.Item;
        await Navigation.PushAsync(new TemplateExistingEditorPage(_templateService, _authServices, selectedTemplate));
    }




        private async void btnCreateTemplate_Clicked(object sender, EventArgs e)
    {
        if (await _authServices.PersistentLogin(this) == true)
        {
            await Shell.Current.GoToAsync($"//{nameof(TemplateNewEditorPage)}");
        }
        else
        {
            await DisplayAlert("Error", "You are not connected to the network.", "Exit");
        }
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        LoadTemplates();
    }
}