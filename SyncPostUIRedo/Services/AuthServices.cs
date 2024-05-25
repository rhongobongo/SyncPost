
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyncPostUI.Models;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SyncPostUI.Services
{
    public class AuthServices
    {
        private const string _RefreshToken = "RefreshToken";
        private const string _AccessToken = "AccessToken";
        private const string apiStringPC = "https://localhost:7274/api/Auth/AuthenticationControllers";
        private HttpClient client;  

        public AuthServices()
        {
            client = new HttpClient();
        }

        public async Task<bool> LoginAsync(string userName , string passWord, Page page)
        {
            var loginCredentials = new { username = userName, password = passWord };
            var response = await client.PostAsJsonAsync(apiStringPC + "/Login", loginCredentials);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenModel>(content);

                    Preferences.Remove(_AccessToken);
                    Preferences.Remove(_RefreshToken);
                    Preferences.Set(_RefreshToken, token.RefreshToken);
                    Preferences.Set(_AccessToken, token.AccessToken);
                    return true;
                }
                else
                {
                    await page.DisplayAlert("Error", "Incorrect Credentials" + response, "Close");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PersistentLogin(Page page)
        {
            var rToken = new { refreshToken = Preferences.Get(_RefreshToken, "") };

            try
            {
            var response = await client.PostAsJsonAsync(apiStringPC + "/PersistentLogin", rToken);
            if (rToken.refreshToken == "")
            {
                return false;
            }
                if (response.IsSuccessStatusCode)
            {

                Preferences.Remove(_AccessToken);

                var accessTokenResponse = await response.Content.ReadAsStringAsync();
                JObject responseObject = JObject.Parse(accessTokenResponse);

                // Extract the access token
                string accessToken = (string)responseObject["result"];
                Preferences.Set(_AccessToken, accessToken);

                return true;
            }
            else
            {
                return false;
            }
            }
            catch
            {
                await page.DisplayAlert("Error", "You are not connected to the network", "Close");
                //await PersistentLogin(page);
                return false;
            }
        }

        public async void Logout()
        {
            var rToken = new { refreshToken = Preferences.Get(_RefreshToken, "") };
            try
            {
                var httpClientHandler = new HttpClientHandler();
             

                client = new HttpClient(httpClientHandler);
                var content = new StringContent(JsonConvert.SerializeObject(rToken), Encoding.UTF8, "application/json");
                var response = await client.DeleteAsync(apiStringPC + "/Logout?refreshToken=" + rToken.refreshToken);

                if (response.IsSuccessStatusCode)
                {
                    Preferences.Remove(_AccessToken);
                    Preferences.Remove(_RefreshToken);
                }
                else
                {
                    
                }
            }
            catch
            {
               
            }
        }

        public async Task<bool> Register(RegistrationModel registrationModel)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(registrationModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiStringPC + "/Register", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePassword(ChangePWModel changePWModel, Page page)
        {
            await PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }
            var content = new StringContent(JsonConvert.SerializeObject(changePWModel), Encoding.UTF8, "application/json");

              
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.PatchAsync(apiStringPC + "/ChangePassword", content);
                if (response.IsSuccessStatusCode)
                {
                    await page.DisplayAlert("Success", "The password is changed successfully", "Exit");
                    return true;
                }
                else
                {
                    await page.DisplayAlert("Error", "Incorrect Credentials", "Exit");
                    return false;
                }
            }
            catch
            {
                await page.DisplayAlert("Error", "Connectiontion Error", "Exit");
                return false;
            }
        }


        public string Token()
        {
            return Preferences.Get(
                _AccessToken,"");
        }
    }
}
