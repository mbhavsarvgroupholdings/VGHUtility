using MessageMedia.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VGHUtility_API.Models
{
    public class MessageStatusResponse : GetMessageStatusResponse
    {
        public string DeliveryStatus { get; set; }
             
    }
}
