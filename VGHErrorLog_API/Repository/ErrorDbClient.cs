using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using VGHErrorLog_API.Model;
using VGHErrorLog_API.Utility;

namespace VGHErrorLog_API.Repository
{
    public class ErrorDbClient
    {
        public string SaveLogs(LogErrorModel model, string connString)
        {
            var outParam = new SqlParameter("@ReturnCode", SqlDbType.NVarChar, 20)
            {
                Direction = ParameterDirection.Output
            };
            SqlParameter[] param = {
                new SqlParameter("@appName",model.appName),
                new SqlParameter("@toEmail",model.toEmail),
                new SqlParameter("@error",model.error),
                new SqlParameter("@date",model.date),
                new SqlParameter("@sendEmail",model.sendEmail),
                new SqlParameter("@className",model.className),
                new SqlParameter("@methodName",model.methodName),
                outParam
            };
            SqlHelper.ExecuteProcedureReturnString(connString, "SaveLogError", param);
            return (string)outParam.Value;
        }

        public List<LogErrorModel> GetAllLogs(string connString)
        {
            return SqlHelper.ExtecuteProcedureReturnData<List<LogErrorModel>>(connString,
                "GetAllLogs", r => r.TranslateAsUsersList());
        }
    }
}
