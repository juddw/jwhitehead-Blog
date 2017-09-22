using jwhitehead_Blog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jwhitehead_Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Posts");
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
                    var from = "MyPortfolio<antonio@raynor.com>";
                    var subject = "Blog Contact Email: (no subject)";
                    if (model.Subject != null)
                    {
                        subject = "Blog Contact Email: " + model.Subject;
                    }
                    if (model.Body != null)
                    {
                        body = model.Body + "<p>This is a message from your portfolio site.  The name and the email of the contacting person is above.</p>";
                    }
                    model.Body = "<i>This is a message from your portfolio site.  The name and the email of the contacting person is above.</i>";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return RedirectToAction("Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}