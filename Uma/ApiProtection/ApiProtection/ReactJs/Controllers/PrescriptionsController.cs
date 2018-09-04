using ApiProtection.ReactJs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiProtection.ReactJs.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IJwsParser _jwsParser;

        public PrescriptionsController(IIdentityServerUmaClientFactory identityServerUmaClientFactory, IIdentityServerClientFactory identityServerClientFactory,
            IJwsParser jwsParser)
        {
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _identityServerClientFactory = identityServerClientFactory;
            _jwsParser = jwsParser;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionRequest addPrescriptionRequest)
        {
            string accessToken;
            if (!TryGetAccessToken(out accessToken))
            {
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }

            var doctorSubject = _jwsParser.GetPayload(accessToken)["sub"];
            addPrescriptionRequest.DoctorSubject = doctorSubject.ToString();
            var umaAccessToken = string.Empty;
            umaAccessToken = await TryGetUmaAccessToken(accessToken, GetResourceId(addPrescriptionRequest.PatientSubject), "write");
            if (string.IsNullOrWhiteSpace(umaAccessToken))
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(addPrescriptionRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(json),
                RequestUri = new System.Uri($"{Constants.BaseApiUrl}/prescriptions")
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Add("Authorization", "Bearer " + umaAccessToken);
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            return new ContentResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPrescriptions()
        {
            string accessToken;
            if (!TryGetAccessToken(out accessToken))
            {
                return GetError("not_authorized", "not authorized", HttpStatusCode.Unauthorized);
            }

            var doctorSubject = _jwsParser.GetPayload(accessToken)["sub"].ToString();
            var umaAccessToken = string.Empty;
            umaAccessToken = await TryGetUmaAccessToken(accessToken, GetResourceId(doctorSubject), "read");
            if (string.IsNullOrWhiteSpace(umaAccessToken))
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new System.Uri($"{Constants.BaseApiUrl}/prescriptions/{doctorSubject}")
            };
            request.Headers.Add("Authorization", "Bearer " + umaAccessToken);
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            return new OkObjectResult(JsonConvert.DeserializeObject(json));
        }

        private static string GetResourceId(string subject)
        {
            if(subject == "patient1")
            {
                return "1";
            }

            if (subject == "patient2")
            {
                return "2";
            }

            return null;
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

        private async Task<string> TryGetUmaAccessToken(string idToken, string umaResourceId, string scope)
        {
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma_protection")
                .ResolveAsync(Constants.AuthWellKnownConfiguration);
            if (grantedToken.ContainsError)
            {
                return null;
            }

            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
            {
                ResourceSetId = umaResourceId,
                Scopes = new[] { scope }
            }, Constants.AuthWellKnownConfiguration, grantedToken.Content.AccessToken);
            if (permissionResponse.ContainsError)
            {
                return null;
            }

            var umaAccessToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UseTicketId(permissionResponse.Content.TicketId, idToken)
                .ResolveAsync(Constants.AuthWellKnownConfiguration);
            if (umaAccessToken.ContainsError)
            {
                return null;
            }

            return  umaAccessToken.Content.AccessToken;
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
