using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VGHErrorLog_API.Model;
using VGHErrorLog_API.Utility;

namespace VGHErrorLog_API.Repository
{
    public static class LogsTranslator
    {
        public static LogErrorModel TranslateAsUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }
            var item = new LogErrorModel();

            if (reader.IsColumnExists("appName"))
                item.appName = SqlHelper.GetNullableString(reader, "appName");

            if (reader.IsColumnExists("toEmail"))
                item.toEmail = SqlHelper.GetNullableString(reader, "toEmail");

            if (reader.IsColumnExists("error"))
                item.error = SqlHelper.GetNullableString(reader, "error");

            if (reader.IsColumnExists("date"))
                item.date = SqlHelper.GetNullableString(reader, "date");

            if (reader.IsColumnExists("sendEmail"))
                item.sendEmail = SqlHelper.GetBoolean(reader, "sendEmail");

            if (reader.IsColumnExists("className"))
                item.className = SqlHelper.GetNullableString(reader, "className");

            if (reader.IsColumnExists("methodName"))
                item.methodName = SqlHelper.GetNullableString(reader, "methodName");

            return item;
        }
        public static List<LogErrorModel> TranslateAsUsersList(this SqlDataReader reader)
        {
            var list = new List<LogErrorModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsUser(reader, true));
            }
            return list;
        }
    }
}
