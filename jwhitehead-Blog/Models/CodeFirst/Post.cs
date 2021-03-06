﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jwhitehead_Blog.Models.CodeFirst
{
    public class Post
    {
        public Post()
        {
            // HashSet is a way to organize lists.
            // Giving new instance to the virtual ICollection class below and putting
            // it in the Comments variable
            Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public DateTime Created { get; set; }
        public DateTime? UpdatedDate { get; set; } //question mark means it is now nullable.
        public string MediaUrl { get; set; }
        // add then update database.
        public bool Published { get; set; } // if published it will be true and visible to public, if not false only visible to admin.
        public string Slug { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}