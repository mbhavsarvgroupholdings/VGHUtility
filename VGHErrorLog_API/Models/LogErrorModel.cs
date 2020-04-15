using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VGHErrorLog_API.Model
{
    [DataContract]
    public class LogErrorModel
    {

        [DataMember(Name = "appName")]
        public string appName { get; set; }

        [DataMember(Name = "toEmail")]
        public string toEmail { get; set; }

        [DataMember(Name = "error")]
        public string error { get; set; }

        [DataMember(Name = "date")]
        public string date { get; set; }

        [DataMember(Name = "sendEmail")]
        public bool sendEmail { get; set; }

        [DataMember(Name = "className")]
        public string className { get; set; }

        [DataMember(Name = "methodName")]
        public string methodName { get; set; }
    }
}
