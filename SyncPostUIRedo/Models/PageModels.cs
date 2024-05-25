using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class PictureData
    {
        public string Url { get; set; }
    }

    public class Picture
    {
        public PictureData Data { get; set; }
    }

    public class AccountData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("picture")]
        public Picture Picture { get; set; }
    }

    public class PagingCursors
    {
        public string Before { get; set; }
        public string After { get; set; }
    }

    public class Paging
    {
        public PagingCursors Cursors { get; set; }
    }

    public class AccountsResponse
    {
        public List<AccountData> Data { get; set; }
        public Paging Paging { get; set; }
    }

    public class PageModel
    {
       public string PageName { get; set; }
       public string PageAccessToken { get; set; }
       public string PagePictureURL { get; set; }

       public PageModel(string pageName, string pageAccessToken, string pagePictureURL)
        {
            PageName = pageName;
            PageAccessToken = pageAccessToken;
            PagePictureURL = pagePictureURL;
        }
    }


}
