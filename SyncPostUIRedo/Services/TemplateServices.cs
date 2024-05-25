
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyncPostUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Services
{
    public class TemplateServices
    {
        private const string _AccessToken = "AccessToken";
        private const string apiStringPC = "https://localhost:7274/api/TemplateControllers/Template/";
        //https://localhost:7274/api/TemplateControllers/Template/GetMultipleTemplates
        private readonly AuthServices auth;
        private HttpClient client;

        public TemplateServices(AuthServices authService)
        {
            client = new HttpClient();
            auth = authService;
        }

        public async Task<List<TemplateModel>> GetTemplates(Page page)
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
                response = await client.GetAsync(apiStringPC + "GetMultipleTemplates");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    List<APITemplateModel> getTemplates = JsonConvert.DeserializeObject<List<APITemplateModel>>(jsonResponse);

                    // Convert GetTemplateModel list to TemplateModel list
                    List<TemplateModel> templates = getTemplates.Select(t => new TemplateModel
                    {
                        TemplateID = t.TemplateID,
                        Title = t.Title,
                        Content = t.Content,
                        last_modification_date = t.last_modification_date,
                        // Split the tags string and convert it into a list
                        template_tags = t.template_tags?.Split(',').ToList()
                    }).ToList();

                    return templates;
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


        public async Task<bool> SaveNewTemplate(CreateTemplateModel createTemplateModel, Page page)
        {
            await auth.PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var content = new StringContent(JsonConvert.SerializeObject(createTemplateModel), Encoding.UTF8, "application/json");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(apiStringPC + "PostTemplate", content);
                if (response.IsSuccessStatusCode)
                {
                    await page.DisplayAlert("Success", "The template is created successfully", "Exit");
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

        public async Task<bool> SaveChangesTemplate(ModifyTemplateModel modifyTemplateModel, Page page)
        {
            await auth.PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var content = new StringContent(JsonConvert.SerializeObject(modifyTemplateModel), Encoding.UTF8, "application/json");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.PatchAsync(apiStringPC + "ModifyTemplate", content);
                if (response.IsSuccessStatusCode)
                {
                    await page.DisplayAlert("Success", "The template is modified successfully", "Exit");
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

        public async Task<bool> DeleteTemplate(DeleteTemplateModel deleteTemplateModel, Page page)
        {
            await auth.PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");

            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var content = new StringContent(JsonConvert.SerializeObject(deleteTemplateModel), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                // Send the content in the request body with the POST method
                response = await client.PostAsync(apiStringPC + "DeleteTemplate", content);
                if (response.IsSuccessStatusCode)
                {
                    await page.DisplayAlert("Success", "The template is deleted successfully", "Exit");
                    return true;
                }
                else
                {
                    await page.DisplayAlert("Error", response.ToString(), "Exit");
                    return false;
                }
            }
            catch
            {
                await page.DisplayAlert("Error", "Connection Error", "Exit");
                return false;
            }
        }

        public async Task<bool> CreatePostHistory(CreatePostHistory postHistory, Page page)
        {
            await auth.PersistentLogin(page);
            string accessToken = Preferences.Get(_AccessToken, "");
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var content = new StringContent(JsonConvert.SerializeObject(postHistory), Encoding.UTF8, "application/json");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("https://localhost:7274/api/HistoryControllers/History/CreatePostHistory", content);
                if (response.IsSuccessStatusCode)
                {
                    //await page.DisplayAlert("Success", "The template is created successfully", "Exit");
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

        public async Task<List<PostHistory>> GetHistories(Page page)
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
                response = await client.GetAsync("https://localhost:7274/api/HistoryControllers/History/GetMultiplePostHistory");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    List<GetPostHistory> histories = JsonConvert.DeserializeObject<List<GetPostHistory>>(jsonResponse);

                    // Convert GetPostHistory list to PostHistory list
                    List<PostHistory> postHistories = histories.Select(history => new PostHistory
                    {
                        PostID = history.PostID,
                        UserID = history.UserID,
                        PostContent = history.PostContent,
                        PostDate = history.PostDate,
                        PostedTo = history.PostedTo?.Split(" , ").ToList(), // Split and convert to List<string>
                        PostLink = history.PostLink?.Split(" , ").ToList() // Split and convert to List<string>
                    }).ToList();

                    return postHistories;
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
