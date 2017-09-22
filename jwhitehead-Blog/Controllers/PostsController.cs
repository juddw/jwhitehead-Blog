// using directives like below are in the controller. Keep you from having to write each time in code.
// using statement you would use for each block of code.
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc; // holds ActionResult 
using jwhitehead_Blog.Models.CodeFirst;
using jwhitehead_Blog.Helpers;
using System.IO;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace jwhitehead_Blog.Controllers
{
    [RequireHttps] // one of the steps to force the page to render secure page.
    public class PostsController : Controller
    {
        // instantiate the db object from IdentityModels.
        // it can find the ApplicationDbContext method because of above namespace "using jwhitehead_Blog.Models.CodeFirst;"
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1); // if page is null, it defaults to page 1.

            //example using Linq to find user with more than 5 comments of a specific user
            //private ApplicationDbContext db = new ApplicationDbContext();
            //public ActionResult Index()
            //{
            //    var userHighComments = db.Users.Where(u => u.Comments.Count >= 5 ).ToList();
            //    ViewBag.userHighComments = userHighComments;
            //    return View();
            //}

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                return View(db.Posts.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
            }

            return View(db.Posts.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize)); // jw: sends list to view
        }

        [HttpPost]
        public ActionResult Index(string searchStr, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1); // if page is null, it defaults to page 1.

            ViewBag.Search = searchStr; // this is a way to hold the search term when mulitple pages are presented in search.
            SearchHelper search = new SearchHelper();
            var blogList = search.IndexSearch(searchStr);

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                return View(blogList.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize)); // jw: sends list to view
            }
            return View(blogList.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
        }

        // GET: Posts/Details/5
        public ActionResult Details(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post blogPost = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        //public ActionResult Details(int? id) // jw: ? means you can pass a "null" value.
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    // expliciting delcaring variable by saying only accept "Post" object.
        //    Post post = db.Posts.Find(id);
        //    if (post == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(post);
        //}

        // GET: Posts/Create
        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        public ActionResult Create()
        {
            return View(); // jw: returns the view (probably a form view to fill out)
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        [HttpPost] // Identifies as a post action when multiple actions have the same name.
        [ValidateAntiForgeryToken] // jw: auto generated token in hidden field. When user submits info it matches userId with token.
                                   // jw: get all information from user's info from form field and puts it in the object variable post.
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL,Published")] Post blogPost, HttpPostedFileBase image)
        {
            // Validation.
            if (image != null && image.ContentLength > 0) // checking to make sure there is a file, and that the file has more than 0 bytes of information.
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format."); // Don't need curly braces with only one line of code.
            }

            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == Slug)) // jw: check for duplicate name in db
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }

                if (image != null)
                {
                    // For Image Upload
                    var filePath = "/assets/images/blog-images/"; // url path
                    var absPath = Server.MapPath("~" + filePath);  // physical file path
                    blogPost.MediaUrl = filePath + image.FileName; // path of the file
                    image.SaveAs(Path.Combine(absPath, image.FileName)); // savesvar filePath = "/helpers/img/";
                }

                blogPost.Slug = Slug;
                blogPost.Created = DateTime.Now; // changed from "DateTimeOffset.Now"
                // jw: add any properties that did not come through here. If you deleted Created above then
                // you bind here example: post.Created = DateTime.Now;
                db.Posts.Add(blogPost); // jw: add object to db
                db.SaveChanges(); // saves
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }


        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Created,UpdatedDate,MediaUrl,Published,Slug")] Post post, string mediaURL, HttpPostedFileBase image)
        {
            // Image Upload Validation.
            if (image != null && image.ContentLength > 0) // checking to make sure there is a file, and that the file has more than 0 bytes of information.
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format."); // Don't need curly braces with only one line of code.
            }

            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified; // jw: if any properties are modified it will save.
                if (image != null)
                {
                    var filePath = "/assets/images/"; // url path
                    var absPath = Server.MapPath("~" + filePath);  // physical file path
                    post.MediaUrl = filePath + image.FileName; // path of the file
                    image.SaveAs(Path.Combine(absPath, image.FileName)); // saves
                }
                else
                {
                    post.MediaUrl = mediaURL;
                }
                post.UpdatedDate = System.DateTime.Now;
                db.SaveChanges(); // jW: even if nothing changes and submit button is hit, it will rewrite the same info to db.
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize(Roles = "Admin")] // this will only allow access to the Admin
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id); // look in the Post Table of the db and find the id
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // added with Mark
        public ActionResult CreateComments([Bind(Include = "Id,Body,BlogPostId")] Comment comment)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                if(!String.IsNullOrWhiteSpace(userId))
                {
                comment.CreationDate = DateTime.Now;
                comment.AuthorId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();

                var post = db.Posts.Find(comment.BlogPostId);
                return RedirectToAction("Details", new { Slug = post.Slug });
                }
            }
            return RedirectToAction("Index");
        }

        // made to be changed or replaced.
        // dispose method called after ActionResult is finished.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}