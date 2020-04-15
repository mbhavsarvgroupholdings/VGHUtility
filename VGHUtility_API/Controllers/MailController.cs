using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VGHUtility_API.Controllers;

namespace VGHUtility_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailController : BaseController
    {
        
        public MailController(ILogger<ErrorLogController> log, IConfiguration configuration) : base(log, configuration)
        {
        }

        // GET api/values/5
        [HttpGet]
        [ActionName("Send")]
        public ActionResult<bool> Send(string toEmail, string name, string subject, string fromName, string body, string Attachfile = "")
        {
            return SendEmail(toEmail, name, subject, body, fromName, Attachfile);
        }
        [HttpGet]
        [ActionName("SendMail")]
        public JsonResult SendMail(string toEmail, string name, string subject, string fromName, string body, string Attachfile = "")
        {
            Boolean result = SendEmail(toEmail, name, subject, body, fromName, Attachfile);
            if (result == true)
            {
                return Json(new { success = true, responseText = "Success" });
            }
            else
            { return Json(new { success = false, responseText = "Fail" }); }
        }
    }
}
