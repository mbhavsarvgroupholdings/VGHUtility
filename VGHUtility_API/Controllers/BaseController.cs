using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VGHUtility_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public readonly ILogger<BaseController> _log;
        public IConfiguration _iconfiguration;

        public BaseController(ILogger<BaseController> log, IConfiguration Configuration)
        {
            _log = log;
            _iconfiguration = Configuration;
        }

        [HttpGet]
        public bool SendEmail(string toEmail, string name, string subject, string body, string fromName, string Attachfile = "")
        {
            _log.Log(LogLevel.Information, "SendEmail Process start.");
            _log.Log(LogLevel.Information, "SendEmail =>" + toEmail);
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "172.16.1.26";
            client.Port = 25;
            MailMessage msg = new MailMessage();
            //msg.IsBodyHtml = true;
            if (fromName.Contains("@"))
            {
                msg.From = new MailAddress(fromName);
            }
            else
            {
                msg.From = new MailAddress(fromName + "@vgroupholdings.com"); 
            }
            if (name != "")
            {
                msg.To.Add(new MailAddress(toEmail, name));
            }
            else
            {
                msg.To.Add(new MailAddress(toEmail));
            }

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;    /* string.Format("<html><head></head><body><b>Message Email</b></body>");*/

            //msg.Attachments.Add(attachment);
            //System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            //contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Rtf;
            //contentType.Name = "VGHUtility-20191011.txt";
            //msg.Attachments.Add(new Attachment("d:\\somepath.pdf", contentType));

            //byte[] B = Encoding.UTF8.GetBytes(Attachfile);
            //Attachment att = new Attachment(new MemoryStream(B), "xyz.pdf");
            try
            {
                System.Net.Mail.Attachment attachment;

                if (!string.IsNullOrEmpty(Attachfile))
                {
                    attachment = new System.Net.Mail.Attachment(Attachfile);
                    // attachment = new System.Net.Mail.Attachment("D:\\Bhavesh\\somepath.pdf");
                    msg.Attachments.Add(attachment);
                    client.Send(msg);
                    attachment.Dispose();

                }
                else
                {
                    client.Send(msg);
                }
           
                return true;
            }
            catch (Exception ex)
            {
               // attachment.Dispose();
                _log.Log(LogLevel.Information, "Email error while sending.=>" + ex.Message);
                return false;
            }
        }
        [HttpGet]
        public bool SendEmailMedia(string toEmail, string name, string subject, string body, string fromName, string Attachfile = "")
        {
            _log.Log(LogLevel.Information, "SendEmail Process start.");
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "172.16.1.26";
            client.Port = 25;
            MailMessage msg = new MailMessage();
            //msg.IsBodyHtml = true;
            string Bcc= _iconfiguration["Bcc"];
           // string CC = _iconfiguration["CC"];
            msg.From = new MailAddress(fromName);
            if (name != "")
            {
                msg.To.Add(new MailAddress(toEmail,name));
                msg.Bcc.Add(new MailAddress(Bcc, name));
               // msg.CC.Add(new MailAddress(CC, name));
            }
            else
            {
                msg.To.Add(new MailAddress(toEmail));
            }

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;    /* string.Format("<html><head></head><body><b>Message Email</b></body>");*/

            //msg.Attachments.Add(attachment);
            //System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            //contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Rtf;
            //contentType.Name = "VGHUtility-20191011.txt";
            //msg.Attachments.Add(new Attachment("d:\\somepath.pdf", contentType));

            //byte[] B = Encoding.UTF8.GetBytes(Attachfile);
            //Attachment att = new Attachment(new MemoryStream(B), "xyz.pdf");
            if (!string.IsNullOrEmpty(Attachfile))
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Attachfile);
                // attachment = new System.Net.Mail.Attachment("D:\\Bhavesh\\somepath.pdf");
                msg.Attachments.Add(attachment);
            }
            try
            {
                client.Send(msg);
                _log.Log(LogLevel.Information, "SendEmail Process end.");
                return true;
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "Email error while sending.=>" + ex.Message);
                return false;
            }
        }
    }
}