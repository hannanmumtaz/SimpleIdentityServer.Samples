using ApiProtection.MedicalPrescriptionsApi.DTOs.Requests;
using ApiProtection.MedicalPrescriptionsApi.Extensions;
using ApiProtection.MedicalPrescriptionsApi.Parameters;
using ApiProtection.MedicalPrescriptionsApi.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiProtection.MedicalPrescriptionsApi.Controllers
{
    [Route(Constants.RouteNames.PrescriptionsController)]
    public class PrescriptionsController : Controller
    {
        private readonly IMedicalPrescriptionStore _medicalPrescriptionStore;
        private readonly IMedicalRecordStore _medicalRecordStore;
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public PrescriptionsController(IMedicalPrescriptionStore medicalPrescriptionStore, IMedicalRecordStore medicalRecordStore,
            IAccessTokenStore accessTokenStore, IIdentityServerUmaClientFactory identityServerUmaClientFactory, IIdentityServerClientFactory identityServerClientFactory)
        {
            _medicalPrescriptionStore = medicalPrescriptionStore;
            _medicalRecordStore = medicalRecordStore;
            _accessTokenStore = accessTokenStore;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _identityServerClientFactory = identityServerClientFactory;
        }
        
        [HttpPost]
        // [Authorize("connected")]
        public async Task<IActionResult> Add([FromBody] AddPrescriptionRequest addPrescriptionRequest)
        {
            if (addPrescriptionRequest == null)
            {
                return GetError("invalid_request", "no request in http body", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(addPrescriptionRequest.PatientSubject))
            {
                return GetError("invalid_request", "parameter patient_subject is missing", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(addPrescriptionRequest.Description))
            {
                return GetError("invalid_request", "parameter description is missing", HttpStatusCode.BadRequest);
            }

            var currentSubject = User.GetSubject();
            var medicalRecord = await _medicalRecordStore.Get(currentSubject);
            if (medicalRecord == null)
            {
                return new NotFoundResult();
            }

            var parameter = addPrescriptionRequest.ToParameter(currentSubject);
            // CHECK THE UMA AUTHORIZATION POLICY
            return null;
        }

        // private async Task<bool> CheckAccess()
        // {
        //     return null;
        // }

        private async Task<bool> AddUmaResource(AddMedicalPrescriptionParameter parameter)
        {
            var grantedToken = await _accessTokenStore.GetToken(Constants.WellKnownConfiguration, Constants.ClientId, Constants.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                return false;
            }

            var resource = await _identityServerUmaClientFactory.GetResourceSetClient().AddByResolution(new PostResourceSet
            {
                Name = $"all medical prescriptions {parameter.PatientSubject}",
                Type = "all medical prescriptions",
                Scopes = new List<string>
                {
                    "read",
                    "write"
                }
            }, Constants.WellKnownConfiguration, grantedToken.AccessToken);
            if (resource.ContainsError)
            {
                return false;
            }

            var policy = await _identityServerUmaClientFactory.GetPolicyClient().AddByResolution(new PostPolicy
            {
                ResourceSetIds = new List<string> { resource.Content.Id },
                Rules = new List<PostPolicyRule>
                {
                    new PostPolicyRule
                    {
                        Claims = new List<PostClaim>
                        {
                            new PostClaim
                            {
                                Type = "sub",
                                Value = ""
                            }
                        },
                        Scopes = new List<string>
                        {
                            "read",
                            "write"
                        },
                        OpenIdProvider = Constants.OpenidWellKnownConfiguration
                    }
                }
            }, Constants.WellKnownConfiguration, grantedToken.AccessToken);
            if (policy.ContainsError)
            {
                return false;
            }

            _identityServerUmaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
            {

            }, "", "")

            return true;
        }

        private static IActionResult GetError(string code, string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var jObj = new JObject
            {
                { "code", code },
                { "message", message }
            };
            return new ContentResult
            {
                Content = jObj.ToString(),
                StatusCode = (int)statusCode
            };
        }
    }
}