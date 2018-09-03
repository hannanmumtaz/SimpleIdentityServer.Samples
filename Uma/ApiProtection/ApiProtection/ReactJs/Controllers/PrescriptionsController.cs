using ApiProtection.ReactJs.DTOs;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using System.Threading.Tasks;

namespace ApiProtection.ReactJs.Controllers
{
    public class PrescriptionsController
    {
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public PrescriptionsController(IIdentityServerUmaClientFactory identityServerUmaClientFactory, IIdentityServerClientFactory identityServerClientFactory)
        {
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _identityServerClientFactory = identityServerClientFactory;
        }

        [HttpPost]
        public Task AddPrescription([FromBody] AddPrescriptionRequest addPrescriptionRequest)
        {
            return null;
        }
    }
}
