using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VGHUtility_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : BaseController
    {
        public ErrorLogController(ILogger<ErrorLogController> log, IConfiguration configuration) : base(log,configuration)
        {
            //_log = log;
        }
        // GET: api/ErrorLog/5
        [HttpGet("{id}", Name = "WriteErrorLog")]
        public bool WriteErrorLog(string appName, string toEmail, string error, string fromEmail, string date = "", bool sendEmail = true, string className = "", string methodName = "")
        {
            _log.Log(LogLevel.Information, "WriteErrorLog Process start.");
            if (date == "")
            {
                date = DateTime.Now.Date.ToString();
            }

            if (sendEmail == true)
            {
                ///TODO Save code to DB

                string subject = appName + " - Error occured";
                string body = "Class Name = " + className + "/n" + "Method Name = " + methodName + "/n" + "Error Occured = " + error;
                return SendEmail(toEmail, "", subject, body, fromEmail);
            }
            return false;
        }

    }
}
