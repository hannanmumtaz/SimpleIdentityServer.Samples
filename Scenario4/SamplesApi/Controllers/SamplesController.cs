using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ClientApi.Controllers
{
    [Route("api/samples")]
    public class SamplesController : Controller
    {
        #region Public methods

        [HttpGet]
        [Authorize("readsamples")]
        public IActionResult Get()
        {
            var jObj = new JObject();
            jObj.Add("id", "1234567");
            return new JsonResult(jObj);
        }

        #endregion
    }
}
