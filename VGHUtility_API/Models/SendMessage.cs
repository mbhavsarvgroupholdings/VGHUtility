using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VGHUtility_API.Model
{
    public class SendMessage
    {
        private string _Name;
        [DataMember(Name = "Name")]
        public string Name
        {
            get
            {
                if (Convert.ToString(_Name) == null)
                    return string.Empty;
                else return _Name;
            }
            set { _Name = value; }
        }

        //[DataMember(Name = "ContactNumber")]
        //public string ContactNumber { get; set; }
        private string _Message;
        [DataMember(Name = "Message")]
        public string Message
        {
            get
            {
                if (Convert.ToString(_Message) == null)
                    return string.Empty;
                else return _Message;
            }
            set { _Message = value; }
        }
        private string _Email;
        [DataMember(Name = "Email")]
        public string Email
        {
            get
            {
                if (Convert.ToString(_Email) == null)
                    return string.Empty;
                else return _Email;
            }
            set { _Email = value; }
        }
        private string _Phone;
        [DataMember(Name = "Phone")]
        public string Phone
        {
            get
            {
                if (Convert.ToString(_Phone) == null)
                    return string.Empty;
                else return _Phone;
            }
            set { _Phone = value; }
        }
        private string _Age_Range;
        [DataMember(Name = "Age_Range")]
        public string Age_Range
        {
            get
            {
                if (Convert.ToString(_Age_Range) == null)
                    return string.Empty;
                else return _Age_Range;
            }
            set { _Age_Range = value; }
        }
        private string _ZipCode;
        [DataMember(Name = "ZipCode")]
        public string ZipCode
        {
            get
            {
                if (Convert.ToString(_ZipCode) == null)
                    return string.Empty;
                else return _ZipCode;
            }
            set { _ZipCode = value; }
        }
        private string _Closest_Center;
        [DataMember(Name = "Closest_Center")]
        public string Closest_Center
        {
            get
            {
                if (Convert.ToString(_Closest_Center) == null)
                    return string.Empty;
                else return _Closest_Center;
            }
            set { _Closest_Center = value; }
        }

        //[DataMember(Name = "Created_Date")]
        //public DateTime Created_Date { get; set; }
        private string _CreatedBy;
        [DataMember(Name = "CreatedBy")]
        public string CreatedBy
        {
            get
            {
                if (Convert.ToString(_CreatedBy) == null)
                    return string.Empty;
                else return _CreatedBy;
            }
            set { _CreatedBy = value; }

        }

        private string _AppointmentDate;
        [DataMember(Name = "AppointmentDate")]
        public string AppointmentDate
        {
            get
            {
                if (Convert.ToString(_AppointmentDate) == null)
                    return string.Empty;
                else return _AppointmentDate;
            }
            set { _AppointmentDate = value; }
        }

        private string _AppointmentTime;
        [DataMember(Name = "AppointmentTime")]
        public string AppointmentTime
        {
            get
            {
                if (Convert.ToString(_AppointmentTime) == null)
                    return string.Empty;
                else return _AppointmentTime;
            }
            set { _AppointmentTime = value; }
        }

        private string _Location;
        [DataMember(Name = "Location")]
        public string Location
        {
            get
            {
                if (Convert.ToString(_Location) == null)
                    return string.Empty;
                else return _Location;
            }
            set { _Location = value; }
        }

        private string _Language;
        [DataMember(Name = "Language")]
        public string Language
        {
            get
            {
                if (Convert.ToString(_Language) == null)
                    return string.Empty;
                else return _Language;
            }
            set { _Language = value; }
        }


        private string _MessageID;
        [DataMember(Name = "MessageID")]
        public string MessageID
        {
            get
            {
                if (Convert.ToString(_MessageID) == null)
                    return string.Empty;
                else return _MessageID;
            }
            set { _MessageID = value; }
        }

        private string _DeliversyStatus;
        [DataMember(Name = "DeliversyStatus")]
        public string DeliversyStatus
        {
            get
            {
                if (Convert.ToString(_DeliversyStatus) == null)
                    return string.Empty;
                else return _DeliversyStatus;
            }
            set { _DeliversyStatus = value; }
        }

        private int _Site;
        public int Site {
            get
            {
                if (Convert.ToString(_Site) == null)
                    return 0;
                else return _Site;
            }
            set { _Site = value; }
        } 
    }

    public class BookingConfirmationModel
    {

        private string _textMessage;
        [DataMember(Name = "textMessage")]
        public string textMessage
        {
            get
            {
                if (Convert.ToString(_textMessage) == null)
                    return string.Empty;
                else return _textMessage;
            }
            set { _textMessage = value; }
        }
        private string _timetosend;
        [DataMember(Name = "timetosend")]
        public string timetosend
        {
            get
            {
                if (Convert.ToString(_timetosend) == null)
                    return string.Empty;
                else return _timetosend;
            }
            set { _timetosend = value; }
        }
        private string _primary_phone;
        [DataMember(Name = "primary_phone")]
        public string primary_phone
        {
            get
            {
                if (Convert.ToString(_primary_phone) == null)
                    return string.Empty;
                else return _primary_phone;
            }
            set { _primary_phone = value; }
        }
        private string _lKey;
        [DataMember(Name = "lKey")]
        public string lKey
        {
            get
            {
                if (Convert.ToString(_lKey) == null)
                    return string.Empty;
                else return _lKey;
            }
            set { _lKey = value; }
        }
        
    }

    public class NobleHistoryModel
    {

        private string _MessageResponseID;
        [DataMember(Name = "MessageResponseID")]
        public string MessageResponseID
        {
            get
            {
                if (Convert.ToString(_MessageResponseID) == null)
                    return string.Empty;
                else return _MessageResponseID;
            }
            set { _MessageResponseID = value; }
        }
        private string _MessageSendSuccess;
        [DataMember(Name = "MessageSendSuccess")]
        public string MessageSendSuccess
        {
            get
            {
                if (Convert.ToString(_MessageSendSuccess) == null)
                    return string.Empty;
                else return _MessageSendSuccess;
            }
            set { _MessageSendSuccess = value; }
        }

       // private string _lKey;
        [DataMember(Name = "lKey")]
        public Int32 lKey
        {
            get;set;
            //get
            //{
            //    if (Convert.ToString(_lKey) == null)
            //        return string.Empty;
            //    else return _lKey;
            //}
            //set { _lKey = value; }
        }

    }
}
