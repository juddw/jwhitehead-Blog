using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jwhitehead_Blog.Models.CodeFirst
{
    public class Comment
    {
        // Created these
        public int Id { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; } //question mark means it is now nullable.
        public string UpdateReason { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }

        public virtual Post BlogPost { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}