using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace CommonUtil
{
    public static class VGHServices
    {
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="toEmail">Email address to whom you want to sent</param>
        /// <param name="name"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Boolean SendEmail(string toEmail, string name, string subject, string body)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44322/api/");
                //HTTP GET
                var responseTask = client.GetAsync(string.Format("Mail/Send?toEmail={0}&name={1}&subject={2}&body={3}", toEmail, name, subject, body));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean ErrorLogs(string appName, string toEmail, string error, string date = "", bool sendEmail = true, string className = "", string methodName = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44322/api/");
                //HTTP GET
                var responseTask = client.GetAsync(string.Format("Mail/Send?appName={0}&date={1}&error={2}&sendEmail={3}&toEmail={4}&className={5}&methodName={6}", appName, date, error, sendEmail, toEmail, className, methodName));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
