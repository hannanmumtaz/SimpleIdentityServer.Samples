using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Belg.Auth.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        #region Public methods

        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "client_1", "client_2" };
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            var user = User;
            return "coucou";
        }

        #endregion
    }
}
