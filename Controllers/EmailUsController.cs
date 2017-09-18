using AT.Net.Service;
using EmailTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailTest.Controllers
{
    public class EmailUsController : Controller
    {
        // GET: EmailUs
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmailUs(HttpPostedFileBase file, EmailUsModel email)
        {
            List<string> _To = email.To.Split(';').ToList();
            List<string> _Cc = string.IsNullOrEmpty(email.Cc) ? null : email.Cc.Split(';').ToList();
            List<string> _BCc = string.IsNullOrEmpty(email.Bcc) ? null : email.Bcc.Split(';').ToList();

            Email e = new Email()
            {
                To = _To,
                Cc = _Cc,
                Bcc = _BCc,
                Subject = email.Subject,
                Body = email.Message,
                Format = BodyFormat.HTML,
                Priority = Priority.High
            };

            e.Attachments = new List<Attachment>();
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                string GeneratedFilename = Guid.NewGuid().ToString("N");

                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/_Attachments"),
                    GeneratedFilename + "_" + file.FileName);
                file.SaveAs(path);

                //  Add the attachment into email.
                Attachment FileInfo = new Attachment()
                {
                    Path = "~/_Attachments",
                    Filename = GeneratedFilename + "_" + file.FileName,
                    OriginalFilename = fileName
                };
                e.Attachments.Add(FileInfo);
            }

            e.Send(true);
            return RedirectToAction("Index");
        }
    }
}