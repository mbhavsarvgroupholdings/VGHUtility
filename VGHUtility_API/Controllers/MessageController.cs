using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageMedia.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MessageMedia.Messages.Controllers;
using MessageMedia.Messages.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VGHUtility_API.Model;
using VGHUtility_API.Controllers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using VGHUtility_API.Models;

namespace VGHUtility_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : BaseController
    {
        public MessageController(ILogger<ErrorLogController> log, IConfiguration configuration) : base(log, configuration)
        {
        }

        // GET api/values/5
        /// <summary>
        /// Send SMS 
        /// </summary>
        /// <param name="sourceApplication">Name of application from where sending SMS</param>
        /// <param name="content">Message Content</param>
        /// <param name="destination_number">Phone number in numeric format only</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Send")]
        [Consumes("application/x-www-form-urlencoded", "application/json")]
        [Produces("application/json")]
        public ActionResult<bool> Send([FromForm] string sourceapplication, [FromForm] string destination_number, [FromForm] string content)
        {
            try
            {
                ///TODO : 1. Validate input. If blank reply false
                ///2. Set keys in appsettings for below two key
                ///3. Handle Log
                ///4. SEt from Name

                _log.Log(LogLevel.Information, "Send SMS Process start.");
                var basicAuthUserName = _iconfiguration["basicAuthUserName"];
                var basicAuthPassword = _iconfiguration["basicAuthPassword"];
                var FromName = _iconfiguration["FromName"];

                //String basicAuthUserName = "RRk8fAWgUZIg8cHu4Lar";
                //String basicAuthPassword = "93fGt52hbNLpAiT1nB16xdUaHuWDVR";
                bool useHmacAuthentication = false;

                MessageMediaMessagesClient client = new MessageMediaMessagesClient(basicAuthUserName, basicAuthPassword, useHmacAuthentication);
                MessagesController messages = client.Messages;

                string bodyValue = "{";
                bodyValue += "'messages':[";
                bodyValue += "{";
                bodyValue += "'content':'" + content + "',";
                bodyValue += "'destination_number':'" + destination_number + "',";
                bodyValue += "'source_number': 'VGH',";
                bodyValue += "'source_number_type': 'ALPHANUMERIC'";
                bodyValue += "}";
                bodyValue += "]";
                bodyValue += "}";
                var body = Newtonsoft.Json.JsonConvert.DeserializeObject<SendMessagesRequest>(bodyValue);
                SendMessagesResponse result = messages.SendMessages(body);
                if (result.Messages.Count > 0)
                {
                    _log.Log(LogLevel.Information, result.Messages[0].Status.Value.ToString());
                    _log.Log(LogLevel.Information, result.Messages[0].MessageId.ToString());
                    _log.Log(LogLevel.Information, result.Messages[0].DestinationNumber.ToString());
                }
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "SMS error while sending.");
                return false;
            }

            return true;
        }


        [HttpPost]
        [ActionName("SendMessagetoMedia")]
        [Consumes("application/x-www-form-urlencoded", "application/json")]
        [Produces("application/json")]
        public async Task<string> SendMessagetoMedia([FromBody]SendMessage model)
        {
            string status = "success";
            try
            {
                //
                ///TODO : 1. Validate input. If blank reply false
                ///2. Set keys in appsettings for below two key
                ///3. Handle Log
                ///4. SEt from Name
                string s = JsonConvert.SerializeObject(model);
                _log.Log(LogLevel.Information, "SendMessagetoMedia XML object :: " + s.ToString());

                string startTime = _iconfiguration["StartTime"];
                string endTime = _iconfiguration["EndTime"];
                if (model.Site != 67)
                {
                    _log.Log(LogLevel.Information, "Site other than 67.");
                    return status = "Fail,Site other than 67 but it is " + model.Site.ToString();
                }
                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday &&
                    DateTime.Now.DayOfWeek != DayOfWeek.Sunday &&
                    DateTime.Now < Convert.ToDateTime(DateTime.Now.ToLongDateString()+ startTime)
                    ||
                    DateTime.Now > Convert.ToDateTime(DateTime.Now.ToLongDateString() + endTime))

                {
                    _log.Log(LogLevel.Information, "Current time is out of range.");
                    return status = "Fail,Current time is out of range. Current time is " + DateTime.Now.ToString();
                }
                if (model.Phone.Length > 10 || model.Phone.Length < 10)
                {
                    _log.Log(LogLevel.Information, "Error:Phone number length more then 10 character.");
                    return status = "Fail,Please Enter Valid Phone Number.";
                }
                _log.Log(LogLevel.Information, "Send SMS Process start.");
                var basicAuthUserName = _iconfiguration["basicAuthUserName"];
                var basicAuthPassword = _iconfiguration["basicAuthPassword"];
                var FromName = _iconfiguration["FromName"];
                string _defaultMessage = _iconfiguration["defaultMessage"];
                string _customMessage = _iconfiguration["customMessage"];
                string default_to_mailer = _iconfiguration["default_to_mailer"];
                string default_from_mailer = _iconfiguration["CountryCode"] + model.Phone + _iconfiguration["default_from_name"];
                string default_email_body = _iconfiguration["default_email_body"];
                var _messagetext = model.Message;

                if (string.IsNullOrEmpty(_messagetext))
                {
                    if (!string.IsNullOrEmpty(model.AppointmentDate) && !string.IsNullOrEmpty(model.AppointmentTime))
                    {
                        _messagetext = _customMessage;
                    }
                    else
                    {
                        _messagetext = _defaultMessage;
                    }
                }

                bool useHmacAuthentication = false;

                MessageMediaMessagesClient client = new MessageMediaMessagesClient(basicAuthUserName, basicAuthPassword, useHmacAuthentication);
                MessagesController messages = client.Messages;

                string bodyValue = "{";
                bodyValue += "'messages':[";//
                bodyValue += "{";
                bodyValue += "'content':  '" + _messagetext + "',";

                bodyValue += "'destination_number':'" + _iconfiguration["CountryCode"] + model.Phone + "',";
                bodyValue += "'source_number': 'VGH',";
                bodyValue += "'source_number_type': 'ALPHANUMERIC',";
                bodyValue += "'delivery_report': 'true'";
                bodyValue += "}";
                bodyValue += "]";
                bodyValue += "}";
                var body = Newtonsoft.Json.JsonConvert.DeserializeObject<SendMessagesRequest>(bodyValue);
                SendMessagesResponse result = await messages.SendMessagesAsync(body);
                _log.Log(LogLevel.Information, "Send SMS Process end.");
                if (result.Messages.Count > 0)
                {
                    GetMessageStatusResponse _getmessageresponse = messages.GetMessageStatus(result.Messages[0].MessageId.ToString());

                    _log.Log(LogLevel.Information, result.Messages[0].Status.Value.ToString());
                    _log.Log(LogLevel.Information, result.Messages[0].MessageId.ToString());
                    _log.Log(LogLevel.Information, result.Messages[0].DestinationNumber.ToString());
                    _log.Log(LogLevel.Information, "Delivery Status:" + _getmessageresponse.Status.ToString());

                    model.MessageID = result.Messages[0].MessageId.ToString();
                    model.DeliversyStatus = _getmessageresponse.Status.ToString();
                    // _body += model.Name + " " + default_email_body;
                    model.CreatedBy = _iconfiguration["CreatedBy"];
                    OuputLogInformation(model);

                    string emailbody = "";
                    emailbody += "<table >";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Name</strong></td><td>" + model.Name + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Email</strong></td><td>" + model.Email + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Phone</strong></td><td>" + model.Phone + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Age</strong></td><td>" + model.Age_Range + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>ZipCode</strong></td><td>" + model.ZipCode + "</td></tr>";
                    //emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Closest Center</strong></td><td>" + model.Closest_Center + "</td></tr>";
                    // emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Created Date</strong></td><td>" + model.Created_Date + "</td></tr>";
                    //emailbody += "<tr><td style='background-color: yellowgreen;'><strong>CreatedBy</strong></td><td>" + model.CreatedBy + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>AppointmentDate</strong></td><td>" + model.AppointmentDate + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>AppointmentTime</strong></td><td>" + model.AppointmentTime + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Location</strong></td><td>" + model.Location + "</td></tr>";
                    emailbody += "<tr><td style='background-color: yellowgreen;'><strong>Language</strong></td><td>" + model.Language + "</td></tr>";
                    emailbody += "</table>";

                    SendEmailMedia(default_to_mailer, FromName, "SMS Chat [" + model.Name + "-" + model.Phone + "]", emailbody, default_from_mailer);
                }
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "SMS error while sending." + ex.Message);
                return status = "Something went wrong!";
            }
            return status;
        }

        private void OuputLogInformation(SendMessage obj)
        {
            try
            {
                _log.Log(LogLevel.Information, "MessageController => OuputLogInformation start.");

                string InsertOuputLogResult = string.Empty;
                using (var client = new HttpClient())
                {
                    var APIUrl = _iconfiguration["OutputLogURL"];
                    client.BaseAddress = new Uri(APIUrl);
                    //HTTP GET
                    var responseTask = client.PostAsJsonAsync("OutputLog/InsertMessageLogAsync", obj);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        if (string.IsNullOrEmpty(readTask.Result) != true)
                        {
                            InsertOuputLogResult = Newtonsoft.Json.JsonConvert.DeserializeObject(readTask.Result).ToString();
                        }
                    }
                    else
                    {
                        _log.Log(LogLevel.Information, "MessageController => Status Code:" + result.StatusCode);
                    }
                }
                _log.Log(LogLevel.Information, "MessageController => OuputLogInformation end.");
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "MessageController => OuputLogInformation Error =>." + ex.Message);
            }
        }

        [HttpGet]
        [ActionName("SendMessageMediaResponse")]
        [Consumes("application/x-www-form-urlencoded", "application/json")]
        [Produces("application/json")]
        public MessageStatusResponse SendMessageMediaResponse(string MessageID)
        {
            try
            {
                Guid guidResult;
                bool isValid = Guid.TryParse(MessageID, out guidResult);

                if (isValid == true)
                {
                    _log.Log(LogLevel.Information, "SendMessageMediaResponse Start");
                    var basicAuthUserName = _iconfiguration["basicAuthUserName"];
                    var basicAuthPassword = _iconfiguration["basicAuthPassword"];
                    bool useHmacAuthentication = false;

                    MessageMediaMessagesClient client = new MessageMediaMessagesClient(basicAuthUserName, basicAuthPassword, useHmacAuthentication);
                    MessagesController messages = client.Messages;
                    GetMessageStatusResponse _getmessageresponse = messages.GetMessageStatus(MessageID);
                    MessageStatusResponse messageStatusResponse = new MessageStatusResponse() { DeliveryStatus = _getmessageresponse.Status.ToString(), MessageId = _getmessageresponse.MessageId, CallbackUrl = _getmessageresponse.CallbackUrl, Content = _getmessageresponse.Content, DestinationNumber = _getmessageresponse.DestinationNumber, DeliveryReport = _getmessageresponse.DeliveryReport, Format = _getmessageresponse.Format, MessageExpiryTimestamp = _getmessageresponse.MessageExpiryTimestamp, Metadata = _getmessageresponse.Metadata, Scheduled = _getmessageresponse.Scheduled, SourceNumber = _getmessageresponse.SourceNumber, SourceNumberType = _getmessageresponse.SourceNumberType };
                    _log.Log(LogLevel.Information, "SendMessageMediaResponse End");
                    return messageStatusResponse;
                }
                else
                {
                    _log.Log(LogLevel.Information, "Invalid GUID");
                    return null;
                }

            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "SendMessageMediaResponse SMS reply error while sending." + ex.Message);
                return null;
            }
        }

        [HttpGet]
        [ActionName("SendSMSBookingConfirmation")]
        [Consumes("application/x-www-form-urlencoded", "application/json")]
        [Produces("application/json")]
        public async Task<string> SendSMSBookingConfirmation()
        {
            string status = "success";
            try
            {
                _log.Log(LogLevel.Information, "Send SMS Process start.");
                var basicAuthUserName = _iconfiguration["basicAuthUserName"];
                var basicAuthPassword = _iconfiguration["basicAuthPassword"];
                var FromName = _iconfiguration["FromName"];

                List<BookingConfirmationModel> _models = GetSMSBookingConfirmationDetails();

                foreach (var model in _models)
                {

                    try
                    {
                        bool useHmacAuthentication = false;

                        MessageMediaMessagesClient client = new MessageMediaMessagesClient(basicAuthUserName, basicAuthPassword, useHmacAuthentication);
                        MessagesController messages = client.Messages;

                        string bodyValue = "{";
                        bodyValue += "'messages':[";
                        bodyValue += "{";
                        bodyValue += "'content':  '" + model.textMessage + "',";
                        bodyValue += "'destination_number':'" + _iconfiguration["CountryCode"] + model.primary_phone + "',";
                        bodyValue += "'source_number': 'VGH',";
                        bodyValue += "'source_number_type': 'ALPHANUMERIC'";
                        bodyValue += "}";
                        bodyValue += "]";
                        bodyValue += "}";

                        _log.Log(LogLevel.Information, "SendSMSBookingConfirmation  object String :: " + bodyValue.ToString());
                        var body = Newtonsoft.Json.JsonConvert.DeserializeObject<SendMessagesRequest>(bodyValue);
                        SendMessagesResponse result = await messages.SendMessagesAsync(body);
                        _log.Log(LogLevel.Information, "Send SMS Process end.");
                        if (result.Messages.Count > 0)
                        {
                            GetMessageStatusResponse _getmessageresponse = messages.GetMessageStatus(result.Messages[0].MessageId.ToString());

                            _log.Log(LogLevel.Information, "lKey value : " + model.lKey);
                            _log.Log(LogLevel.Information, "Status value : "+ result.Messages[0].Status.Value.ToString());
                            _log.Log(LogLevel.Information, "MessageResponseID value : " +result.Messages[0].MessageId.ToString()); //MessageResponseID
                            _log.Log(LogLevel.Information, "DestinationNumber value : " + result.Messages[0].DestinationNumber.ToString());
                            _log.Log(LogLevel.Information, "Delivery Status:" + _getmessageresponse.Status.ToString()); //MessageSendSuccess

                            NobleHistoryModel objmain = new NobleHistoryModel();
                            objmain.lKey = Convert.ToInt32(model.lKey);
                            objmain.MessageResponseID = result.Messages[0].MessageId.ToString();
                            objmain.MessageSendSuccess = _getmessageresponse.Status.ToString();
                            string Res = UpdateNobleHistorySMSStatus(objmain);

                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Log(LogLevel.Information, "error occured while sending Confirmation  SMS :" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "SendSMSBookingConfirmation() --> SMS error while sending." + ex.Message);
                return status = "Something went wrong!";
            }
            return status;
        }

        private List<BookingConfirmationModel> GetSMSBookingConfirmationDetails()
        {
            try
            {
                //// api call start
                ///
                List<BookingConfirmationModel> _SMSBookingModel = new List<BookingConfirmationModel>();
                using (var client = new HttpClient())
                {
                    _log.Log(LogLevel.Information, "Web API Call Start- get ConfirmationResponse data from CRM Database.");
                    var APIUrl = _iconfiguration["CRMURL"];
                    client.BaseAddress = new Uri(APIUrl);
                    //HTTP GET
                    var responseTask = client.GetAsync(string.Format("CRMLegacy/GetSMSBookingConfirmationDetails"));
                    var result = responseTask;
                    if (result.Result.IsSuccessStatusCode)
                    {
                        var readTask = result.Result.Content.ReadAsAsync<IList<BookingConfirmationModel>>();

                        readTask.Wait();
                        _SMSBookingModel = readTask.Result.ToList();
                    }
                    _log.Log(LogLevel.Information, "Web API Call End.");
                }
                return _SMSBookingModel;
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, ex.Message);
                return new List<BookingConfirmationModel>();
            }
        }

        private string UpdateNobleHistorySMSStatus(NobleHistoryModel objmain)
        {
            try
            {
                _log.Log(LogLevel.Information, "MessageController => UpdateNobleHistorySMSStatus start.");

                string NobleHistoryResult = string.Empty;
                using (var client = new HttpClient())
                {
                    var APIUrl = _iconfiguration["NobleURL"];
                    client.BaseAddress = new Uri(APIUrl);
                    //HTTP Post
                    var responseTask = client.PostAsJsonAsync("NobleHistory/UpdateNobleHistorySMSStatus", objmain);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        NobleHistoryResult = readTask.Result.ToString();
                    }
                }
                _log.Log(LogLevel.Information, "MessageController => UpdateNobleHistorySMSStatus end.");

                return NobleHistoryResult;
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Information, "MessageController => UpdateNobleHistorySMSStatus Error =>." + ex.Message);
                return "";
            }
        }
    }
}