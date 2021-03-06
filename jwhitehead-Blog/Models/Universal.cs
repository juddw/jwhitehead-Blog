﻿using jwhitehead_Blog.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace jwhitehead_Blog.Models
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuted(ActionExecutedContext filterContext) // OnActionExecuting happened
                                                                                      // at the same time as the controller was called. Changing it to OnActionExecuted happens after
        {
            base.OnActionExecuted(filterContext);
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;
            }
        }
    }
}