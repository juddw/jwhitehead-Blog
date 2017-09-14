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

namespace jwhitehead_Blog.Controllers
{
    public class PostsController : Controller
    {
        // instantiate the db object from IdentityModels.
        // it can find the ApplicationDbContext method because of above namespace "using jwhitehead_Blog.Models.CodeFirst;"
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList()); // jw: sends list to view
        }

        // GET: Posts/Details/5

        public ActionResult Details(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post blogPost = db.Posts.FirstOrDefault(p => p.Slug == slug);
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
        public ActionResult Create()
        {
            return View(); // jw: returns the view (probably a form view to fill out)
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken] // jw: auto generated token in hidden field. When user submits info it matches userId with token.
                                   // jw: get all information from user's info from form field and puts it in the object variable post.
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL,Published")] Post blogPost)
        {
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

                blogPost.Slug = Slug;
                blogPost.Created = DateTime.Now;
                // jw: add any properties that did not come through here. If you deleted Created above then
                // you bind here example: post.Created = DateTime.Now;
                db.Posts.Add(blogPost); // jw: add object to db
                db.SaveChanges(); // saves
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }


        // GET: Posts/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Created,UpdatedDate,MediaUrl,Published,Slug")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified; // jw: if any properties are modified it will save.
                db.SaveChanges(); // jW: even if nothing changes and submit button is hit, it will rewrite the same info to db.
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id); // look in the Post Table of the db and find the id
            db.Posts.Remove(post);
            db.SaveChanges();
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
