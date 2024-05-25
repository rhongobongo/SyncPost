using Newtonsoft.Json;
using SyncPostUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Services
{
    public class MiscServices
    {
        private const string _AccessToken = "AccessToken";
        private const string apiStringPC = "https://localhost:7274/api/";
        private readonly AuthServices auth;
        private HttpClient client;

        public MiscServices(AuthServices authService)
        {
            client = new HttpClient();
            auth = authService;
        }

        public async Task<string> ShowUsername(Page page)
        {
            await auth.PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");

            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(apiStringPC + "Misc/UsernameReturn/GetUsername");
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Assuming the response body contains just the username as a string
                    return responseBody;
                }
                else
                {
                    await page.DisplayAlert("Error", "Incorrect Credentials", "Exit");
                    return null;
                }
            }
            catch
            {
                await page.DisplayAlert("Error", "Connection Error", "Exit");
                return null;
            }
        }

    }
}
