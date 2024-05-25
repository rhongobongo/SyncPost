using __XamlGeneratedCode__;
using SyncPostUI.Models;
using SyncPostUI.Services;

namespace SyncPostUI.Pages;

public partial class PostingPage : ContentPage
{
    private readonly TemplateServices _templateService;
    private readonly AuthServices _authServices;
    private readonly FacebookServices _fb;
    private readonly APITemplateModel templateModel1;
    private string Prompts;
    private List<string> PostPrompts = new List<string>();
    private List<string> SharePrompts = new List<string>();
    private List<PageModel>? _pageAccounts;

    private async Task<List<PageModel>> GetPageAccountsAsync()
    {
        if (_pageAccounts == null)
        {
            _pageAccounts = await _fb.GetPages(this);
        }
        return _pageAccounts;
    }

    public PostingPage(TemplateServices templateService, AuthServices authServices, APITemplateModel templateModel, FacebookServices facebookServices)
    {
        InitializeComponent();
        _templateService = templateService;
        _authServices = authServices;
        templateModel1 = templateModel;
        _fb = facebookServices;
    }

    protected override async void OnAppearing()
    {
       await GetPageAccountsAsync();
        generateCheckboxes(SVCPost, nameof(SVCPost));
        //generateCheckboxes(SVCShare, nameof(SVCShare));
    }

    private void generateCheckboxes(ScrollView scrollView, string testing)
    {
        // Clear existing content of the grid
        scrollView.Content = null;

        // Create a new grid
        Grid grid = new Grid();

        // Retrieve the list of page accounts asynchronously
        

        // Define columns for the grid
        grid.ColumnSpacing = 20;
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
        grid.ColumnDefinitions.Add(new ColumnDefinition()); 
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        // Define rows for the grid
        for (int i = 0; i < _pageAccounts.Count; i++)
        {
            // Create a new row definition
            RowDefinition row = new RowDefinition();
            grid.RowDefinitions.Add(row);

            // Create a label for the page name
            Image image = new Image();
            // Set the source of the image to a URI obtained from the internet
            image.Source = new Uri(_pageAccounts[i].PagePictureURL);
            image.WidthRequest = 32;
            image.HeightRequest = 32;
            Label label = new Label();
            label.Text = _pageAccounts[i].PageName;

            // Create a checkbox
            CheckBox checkBox = new CheckBox { HorizontalOptions = LayoutOptions.End };
            checkBox.CheckedChanged +=  (sender, e) =>
            {
                // Check if the checkbox is checked
                if (e.Value)
                {
                    if (testing == "SVCPost")
                    {
                        PostPrompts.Add(label.Text);
                    }
                    else if(testing == "SVCShare")
                    {
                        SharePrompts.Add(label.Text);
                    }
                    //await DisplayAlert("Test", "Checkbox is checked!" + label.Text, "Test");
                }
                else
                {
                    if (testing == "SVCPost")
                    {
                        PostPrompts.Remove(label.Text);
                    }
                    else if (testing == "SVCShare")
                    {
                        SharePrompts.Remove(label.Text);
                    }
                    //await DisplayAlert("Test", "Checkbox is unchecked!" + label.Text, "Test");
                }
            };
            // Set grid row for label and checkbox
            Grid.SetRow(image, i);
            Grid.SetRow(label, i);
            Grid.SetRow(checkBox, i);

            // Set column for label and checkbox
            Grid.SetColumn(image, 0);
            Grid.SetColumn(label, 1);
            Grid.SetColumn(checkBox, 2);


            // Add label and checkbox to the grid
            grid.Children.Add(image);
            grid.Children.Add(label);
            grid.Children.Add(checkBox);
        }
        grid.Padding = new Thickness(20, 20, 20, 20); // Sets 10 units of padding on the left and right sides, 20 units on the top, and 30 units on the bottom

        // Set the grid as the content of the ScrollView
        scrollView.Content = grid;
    }

    private async void btnPost_Clicked(object sender, EventArgs e)
    {
        string postedLinks = null;
        string pagesPosted = null;
        foreach(var page in PostPrompts)
        {
            await DisplayAlert("You are going to post to", page, "Close");
            postedLinks += await _fb.PostToFacebook(templateModel1.Content, GetAccessToken(page), this) + " , ";
            pagesPosted += page + " , ";
        }
        var postHis = new CreatePostHistory() { PostContent = templateModel1.Content, PostLink =  postedLinks, PostedTo = pagesPosted};
        await _templateService.CreatePostHistory(postHis, this);

        await DisplayAlert("PostLinks", postedLinks, "Close");
    }

    private string GetAccessToken(string name)
    {
        PageModel page;
        foreach(var pageAccount in _pageAccounts)
        {
            if(pageAccount.PageName == name)
            {
                return pageAccount.PageAccessToken;
            }
        }
        return null;
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}