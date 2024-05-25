using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Models
{
    public class TemplateModel
    {
        public string TemplateID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime last_modification_date { get; set; }
        public List<string>? template_tags { get; set; }
    }
    public class APITemplateModel
    {
        public string TemplateID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime last_modification_date { get; set; }
        public string? template_tags { get; set; }
    }


    public class CreateTemplateModel
    {
        public string template_title { get; set; }

        public string template_content { get; set; }
        public string? template_tags { get; set; }
    }

    public class ModifyTemplateModel
    {
        public string templateID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string? template_tags { get; set; }
    }

    public class DeleteTemplateModel
    {
        public string templateID { get; set; }
    }

    public class CreatePostHistory
    {

        public string PostContent { get; set; }
        public string PostedTo { get; set; }
        public string PostLink { get; set; }

    }

    public class GetPostHistory
    {
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string PostContent { get; set; }

        public DateTime PostDate { get; set; }
        public string PostedTo { get; set; }
        public string PostLink { get; set; }
    }

    public class PostHistory
    {
        public string PostID { get; set; }

        public string UserID { get; set; }

        public string PostContent { get; set; }

        public DateTime PostDate { get; set; }
        public List<string> PostedTo { get; set; }
        public List<string> PostLink { get; set; }
    }
}

