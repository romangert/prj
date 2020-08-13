using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace service0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test0Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            string result;
            WebRequest request = WebRequest.Create("http://service1/api/test1");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
            }

            return result;
        }
    }
}