using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        #region Public methods

        [HttpGet]
        [Authorize("uma")]
        public IEnumerable<string> Get()
        {
            return new string[] { "client_1", "client_2" };
        }

        #endregion
    }
}
