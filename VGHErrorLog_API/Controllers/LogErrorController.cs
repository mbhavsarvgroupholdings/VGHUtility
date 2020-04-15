using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VGHErrorLog_API.Model;
using VGHErrorLog_API.Repository;
using VGHErrorLog_API.Utility;

namespace VGHErrorLog_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LogErrorController : Controller
    {
        IConfiguration _iconfiguration;

        public LogErrorController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveLogError([FromBody]LogErrorModel model)
        {
            string DbConn = _iconfiguration.GetSection("MySettings").GetSection("DbConn").Value;
            var msg = new Message<LogErrorModel>();
            var data = DbClientFactory<ErrorDbClient>.Instance.SaveLogs(model, DbConn);
            if (data == "C200")
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "LogError saved successfully";
            }
            return Ok(msg);
        }

        [HttpGet]
        public IActionResult GetAllLogs()
        {
            string DbConn = _iconfiguration.GetSection("MySettings").GetSection("DbConn").Value;
            var data = DbClientFactory<ErrorDbClient>.Instance.GetAllLogs(DbConn);
            return Ok(data);
        }
    }
}