using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VGHAdsApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ADSController : ControllerBase
    {
        [HttpGet]
        [ActionName("GetAdsProperties")]
        public ActionResult<ADProperties> GetAdsProperties(string userName)
        {
            ADProperties aDProperties = new ADProperties();
            try
            {
                const string LDAP_PATH = "LDAP://10.12.0.8";//"/CN=VCH USERS,DC=MUSA,DC=net";
                                                            //const string LDAP_DOMAIN = "10.12.0.8";//"exldap.example.com:5555";
                //const string FirstName = "Cinthia";
                //const string LastName = "Schram";
                string titleValue = "";
                string departmentValue = "";
                using (var ade = new System.DirectoryServices.DirectoryEntry(LDAP_PATH, "vgh\\ngediya", "Ayush080982"))
                using (var dss = new System.DirectoryServices.DirectorySearcher(ade))
                {
                    // string filter = @"(&(givenname=" + FirstName + ")(sn=" + LastName + "))";
                    string filter = @"(&(sAMAccountName=" + userName + "))";
                    //dss.Filter = "(sAMAccountName=ngediya)";
                    dss.Filter = filter;
                    System.DirectoryServices.SearchResult sresult = dss.FindOne();
                    System.DirectoryServices.DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                    //Console.WriteLine("First Name:" + dsresult.Properties["givenname"][0].ToString());
                    //Console.WriteLine("Last Name:" + dsresult.Properties["cn"][0].ToString());
                    //Console.WriteLine("Full Name:" + dsresult.Properties["name"][0].ToString());
                    //Console.WriteLine("Mail:" + dsresult.Properties["mail"][0].ToString());
                    //Console.WriteLine("LoginId" + dsresult.Properties["sAMAccountName"][0].ToString());

                    foreach (var item in sresult.Properties.PropertyNames)
                    {
                        try
                        {
                            if (dsresult.Properties[item.ToString()].Count > 0)
                            { //Console.WriteLine(item.ToString() + ":" + dsresult.Properties[item.ToString()][0].ToString()); 
                                if (item.ToString() == "title")
                                {
                                    titleValue = dsresult.Properties[item.ToString()][0].ToString();
                                    aDProperties.Title = titleValue;

                                }
                                if (item.ToString() == "department")
                                {
                                    departmentValue = dsresult.Properties[item.ToString()][0].ToString();
                                    aDProperties.Location = departmentValue;
                                }
                                if (item.ToString() == "givenname")
                                {
                                    aDProperties.FirstName = dsresult.Properties[item.ToString()][0].ToString();

                                }
                                if (item.ToString() == "sn")
                                {
                                    aDProperties.LastName = dsresult.Properties[item.ToString()][0].ToString();

                                }
                                if (item.ToString() == "mail")
                                {
                                    aDProperties.EmailAddress = dsresult.Properties[item.ToString()][0].ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Error ----->" + item.ToString());
                            return null;
                        }
                    }

                }

                return aDProperties;
            }
            catch (Exception ex)
            {
                // _log.Log(LogLevel.Information, ex.Message);
                return aDProperties;
            }
        }
        public class ADProperties
        {
            public string Location { get; set; }
            public string Title { get; set; }

            private string _FirstName;
            public string FirstName
            {
                get
                {
                    if (Convert.ToString(_FirstName) == null)
                        return string.Empty;
                    else return _FirstName;
                }
                set { _FirstName = value; }
            }

            private string _LastName { get; set; }
            public string LastName
            {
                get
                {
                    if (Convert.ToString(_LastName) == null)
                        return string.Empty;
                    else return _LastName;
                }
                set { _LastName = value; }
            }
            private string _EmailAddress { get; set; }
            public string EmailAddress
            {
                get
                {
                    if (Convert.ToString(_EmailAddress) == null)
                        return string.Empty;
                    else return _EmailAddress;
                }
                set { _EmailAddress = value; }
            }

        }
    }
}