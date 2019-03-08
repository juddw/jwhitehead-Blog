using jwhitehead_Blog.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jwhitehead_Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Universal
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Posts");
            // return View();
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
                    var from = "MyPortfolio<juddwhitehead@gmail.com>";
                    var subject = "Blog Contact Email: " + model.Subject;

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    // Should be null if human is filling out form cause this is a hidden field.
                    if (model.FromEmail2 == null)
                    {
                        var svc = new PersonalEmail();
                        await svc.SendAsync(email);
                    }

                    // This goes no where so it exits try statement
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