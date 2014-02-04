using System;
using System.Collections.Generic;
using System.Diagnostics;  // for Trace
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealStealUnreal;

namespace DealStealUnreal.Controllers.Admin
{
    public class SettingsController : Controller
    {

        DSURepository db;

        public SettingsController()
        {
            db = new DSURepository();
        }

        [Authorize (Roles = "admin")]
        public ActionResult MailMessage()
        {
            return View("../Admin/MailMessage");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateInput(false)]
        public ActionResult SaveMailMessage(string message)
        {
            //Trace.WriteLine(string.Format("mail message = {0}",message));
            Mailing mail = new Mailing();
            mail.Id = 1;
            mail.date = DateTime.Now.Date;
            mail.message = message;
            mail.status = "register";

            try
            {
                db.Mailings.Add(mail);
                db.SaveChanges();
            }
            catch
            {
                return Content("error"); 
            }
            
            return Content("success"); 
        } 

    }
}
