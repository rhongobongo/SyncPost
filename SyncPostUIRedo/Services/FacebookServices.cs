using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyncPostUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Services
{
    public class FacebookServices
    {
        private const string _FBRefreshToken = "FBRefreshToken";
        private const string _FBAccessToken = "FBAccessToken";
        private readonly string _clientId = "940166921231254";
        private readonly string _redirectUri = "https://localhost:7274/";
        private readonly string _scope = "public_profile, pages_show_list, pages_manage_cta, pages_read_engagement, pages_manage_posts";
        private readonly FacebookClient _facebookClient;

        public FacebookServices()
        {
            _facebookClient = new FacebookClient();
        }
        public async Task<string?> LoginAsync(Page page)
        {
            try
            {
                WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    GetLoginLink(),
                    new Uri(_redirectUri));

                string accessToken = authResult?.AccessToken;

                if(accessToken != null)
                {
                    return accessToken;
                }
                return null;
            }
            catch (TaskCanceledException e)
            {
                await page.DisplayAlert("Error", e.ToString(), "Close");
                return null;
            }
        }

        public Uri GetLoginLink()
        {
            var loginURL = _facebookClient.GetLoginUrl
            (
                new
                {
                    client_id = _clientId,
                    redirect_uri = _redirectUri,
                    scope = _scope
                }
            ) ;

            return loginURL;
        }
        public string OnWebViewNavigating(WebNavigatingEventArgs e, ContentPage signInContentPage)
        {
            if (e.Url.StartsWith("https://localhost"))
            {

                Uri uri = new Uri(e.Url);
                string query = WebUtility.UrlDecode(uri.Query);
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                string? authorizationCode = queryParams.Get("code");
                signInContentPage.Navigation.PopModalAsync();
                return authorizationCode;
            }
            return null;
        }

        public string ExchangeCodeForAccessToken(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                const string tokenUrl = "https://graph.facebook.com/v13.0/oauth/access_token";
                FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", _clientId },
                    { "client_secret", "0209f39b723fb981e6e1d7c8b6408d7f"},
                    { "redirect_uri", _redirectUri },
                    { "grant_type", "authorization_code" }
                });

                // Send a POST request to the token endpoint to exchange the code for an access token
                HttpResponseMessage response = client.PostAsync(tokenUrl, content).Result;

        
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    // Parse the JSON response to extract the access token
                    JObject json = JObject.Parse(responseContent);
                    string accessToken = json.GetValue("access_token").ToString();


                    return accessToken;
                }
                else
                {
                    // Throw an exception or handle the error accordingly
                    throw new Exception($"Token exchange request failed with status code: {response.StatusCode}");
                }
            }
        }

        public async Task<string?> GetAccountConnectedTo(Button button, Page page)
        {
            try
            {
                // Retrieve OAuth token from secure storage
                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

                // Set button background color
                button.BackgroundColor = Color.FromArgb("#ff1877f2");
                button.TextColor = Color.FromArgb("ffffffff");

                // Fetch user account information using the OAuth token
                string userInfo = await GetUserInfo(oauthToken);

                // Example: Parse JSON response to extract user's name
                dynamic userData = JsonConvert.DeserializeObject(userInfo);
                string name = userData.name;
                // Do something with the user's name...

                // Example: Display a message
                return "Logged as " + name;
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., token retrieval failed
                await page.DisplayAlert("Error", "Failed to retrieve user information: " + ex.Message, "OK");
                return null;
            }
        }

        public async Task<string?> GetAccountConnectedTo(Page page)
        {
            try
            {
                // Retrieve OAuth token from secure storage
                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

                // Fetch user account information using the OAuth token
                string userInfo = await GetUserInfo(oauthToken);

                // Example: Parse JSON response to extract user's name
                dynamic userData = JsonConvert.DeserializeObject(userInfo);
                string name = userData.name;
                // Do something with the user's name...

                // Example: Display a message
                return "Logged as " + name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<string> GetUserInfo(string oauthToken)
        {
            try
            {
                // Construct the URL for fetching user information from the Graph API
                string userInfoUrl = $"https://graph.facebook.com/v19.0/me?fields=name,picture&access_token={oauthToken}";

                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to fetch user information
                    HttpResponseMessage response = await client.GetAsync(userInfoUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch user information. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch user information.", ex);
            }
        }

        public async Task<List<PageModel>> GetPages(Page page)
        {
            string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
            string userInfoUrl = $"https://graph.facebook.com/v19.0/me?fields=accounts{{name,access_token,picture{{url}}}}&access_token={oauthToken}";

            var pageModels = new List<PageModel>();
            try
            {
                JObject user = JObject.Parse(await GetUserInfo(oauthToken));
                pageModels.Add(new PageModel((string)user["name"], oauthToken, (string)user["picture"]["data"]["url"]));
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(userInfoUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(jsonResponse);

                        var accountsData = jsonObject["accounts"]["data"];
                        
                        foreach (var account in accountsData)
                        {
                            var pageModel = new PageModel((string)account["name"], (string)account["access_token"], (string)account["picture"]["data"]["url"]);
                            pageModels.Add(pageModel);
                        }
                    }
                    else
                    {
                        await page.DisplayAlert("ERROR", "Failed to retrieve data from the server.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("ERROR", $"An error occurred: {ex.Message}", "OK");
            }

            return pageModels;
        }

        public async Task<string> PostToFacebook(string message, string _accessToken, Page page)
        {
            var fb = new FacebookClient(_accessToken);

            dynamic parameters = new System.Dynamic.ExpandoObject();
            parameters.message = message;

            // Make the post request
            try
            {
                dynamic result = fb.Post("me/feed", parameters);
                //await page.DisplayAlert("SUCCESS","Post ID: " + result.id, "Close");
                return $"https://www.facebook.com/{result.id}";
            }
            catch (FacebookOAuthException ex)
            {
                // Handle OAuth exception
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (FacebookApiException ex)
            {
                // Handle API exception
                Console.WriteLine(ex.Message);
                return null;
            }
        }



    }
}
