using ApiProtection.MedicalPrescriptionsApi.DTOs.Requests;
using ApiProtection.MedicalPrescriptionsApi.Extensions;
using ApiProtection.MedicalPrescriptionsApi.Stores;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IJwsParser _jwsParser;

        public PrescriptionsController(IMedicalPrescriptionStore medicalPrescriptionStore, IMedicalRecordStore medicalRecordStore,
            IAccessTokenStore accessTokenStore, IIdentityServerUmaClientFactory identityServerUmaClientFactory, IIdentityServerClientFactory identityServerClientFactory,
            IJwsParser jwsParser)
        {
            _medicalPrescriptionStore = medicalPrescriptionStore;
            _medicalRecordStore = medicalRecordStore;
            _accessTokenStore = accessTokenStore;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _identityServerClientFactory = identityServerClientFactory;
            _jwsParser = jwsParser;
        }
        
        [HttpPost]
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
            
            var medicalRecord = await _medicalRecordStore.Get(addPrescriptionRequest.PatientSubject);
            if (medicalRecord == null)
            {
                return new NotFoundResult();
            }

            string accessToken;
            if (!TryGetAccessToken(out accessToken))
            {
                Response.Headers.Add("UmaResourceId", medicalRecord.UmaResourceId);
                Response.Headers.Add("UmaWellKnownConfigurationUrl", Constants.WellKnownConfiguration);
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }

            var parameter = addPrescriptionRequest.ToParameter();
            if (!await CheckUmaResource(accessToken, medicalRecord.UmaResourceId, new List<string>
            {
                "write"
            }))
            {
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }

            if (!await _medicalPrescriptionStore.Add(parameter))
            {
                return GetError("internal_error", "an error occured while trying to add the medical prescription", HttpStatusCode.InternalServerError);
            }

            return new ContentResult();
        }

        [HttpGet("{patientSubject}")]
        public async Task<IActionResult> Get(string patientSubject)
        {
            var medicalRecord = await _medicalRecordStore.Get(patientSubject);
            if (medicalRecord == null)
            {
                return new NotFoundResult();
            }

            string accessToken;
            if (!TryGetAccessToken(out accessToken))
            {
                Response.Headers.Add("UmaResourceId", medicalRecord.UmaResourceId);
                Response.Headers.Add("UmaWellKnownConfigurationUrl", Constants.WellKnownConfiguration);
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }
            
            if (!await CheckUmaResource(accessToken, medicalRecord.UmaResourceId, new List<string>
            {
                "read"
            }))
            {
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }

            return new OkObjectResult(medicalRecord);
        }

        private async Task<bool> CheckUmaResource(string accessToken, string exceptedResourceId, ICollection<string> exceptedScopes)
        {
            var introspectionResult = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .Introspect(accessToken, TokenType.AccessToken)
                .ResolveAsync(Constants.WellKnownConfiguration);
            if (introspectionResult.ContainsError || !introspectionResult.Content.Active)
            {
                return false;
            }

            var payload = _jwsParser.GetPayload(accessToken);
            if (!payload.ContainsKey("ticket"))
            {
                return false;
            }

            var tickets = JArray.Parse(payload["ticket"].ToString());
            foreach (JObject ticket in tickets)
            {
                if (!ticket.ContainsKey("resource_id"))
                {
                    continue;
                }

                var resourceId = ticket["resource_id"];
                if (exceptedResourceId == resourceId.ToString())
                {
                    var scopeItems = GetScopes(ticket);
                    foreach (var exceptedScope in exceptedScopes)
                    {
                        if(!scopeItems.Contains(exceptedScope))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private static ICollection<string> GetScopes(JObject jObj)
        {
            var jScopes = jObj["scopes"];
            var jArr = jScopes as JArray;
            if (jArr == null)
            {
                return new List<string>
                {
                    jScopes.ToString()
                };
            }

            return jArr.Select(s => s.ToString()).ToList();
        }

        private bool TryGetAccessToken(out string accessToken)
        {
            accessToken = null;
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authorizationHeader = Request.Headers.FirstOrDefault(h => h.Key == "Authorization");
            if (authorizationHeader.Value.Count != 1)
            {
                return false;
            }

            var splittedAuthorizationHeaderValue = authorizationHeader.Value.First().Split(' ');
            if (splittedAuthorizationHeaderValue.Count() != 2 && splittedAuthorizationHeaderValue[0] != "Bearer")
            {
                return false;
            }

            accessToken = splittedAuthorizationHeaderValue[1];
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