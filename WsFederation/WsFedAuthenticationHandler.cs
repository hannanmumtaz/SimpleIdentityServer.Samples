using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using WsFederation.Messages;

namespace WsFederation
{
    internal class WsFedAuthenticationHandler : RemoteAuthenticationHandler<WsFedAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleRemoteAuthenticateAsync()
        {
            return null;
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            // 1. Redirect to the page
            var returnUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            var signInResponse = new SignInRequestMessage(new Uri(Options.IdPEndpoint), Options.Realm, returnUrl);
            Response.Redirect(signInResponse.RequestUrl);
            return true;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
              && !string.IsNullOrWhiteSpace(Request.ContentType)
              // May have media/type; charset=utf-8, allow partial match.
              && Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
              && Request.Body.CanRead)
            {
                // 1. Retrieve claims from request
                if (!Request.Body.CanSeek)
                {
                    // Buffer in case this body was not meant for us.
                    var memoryStream = new MemoryStream();
                    await Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    Request.Body = memoryStream;
                }

                var form = await Request.ReadFormAsync();
                var collection = new Dictionary<string, StringValues>();
                foreach(var tuple in form)
                {
                    collection.Add(tuple.Key, tuple.Value);
                }

                var uri = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                var wsFederationMessage = WSFederationMessage.CreateFromNameValueCollection(uri, collection);
                var xml = wsFederationMessage.GetParameter("wresult");

                // 2. Parse the XML
                var document = new XmlDocument();
                document.LoadXml(xml);

                var nsMan = new XmlNamespaceManager(document.NameTable);
                nsMan.AddNamespace("trust", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
                nsMan.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
                var parentNodes = "trust:RequestSecurityTokenResponseCollection/trust:RequestSecurityTokenResponse/trust:RequestedSecurityToken/";
                var assertionNode = document.SelectSingleNode(parentNodes + "saml2:EncryptedAssertion", nsMan);
                if (assertionNode == null)
                {
                    assertionNode = document.SelectSingleNode(parentNodes + "saml2:Assertion", nsMan);
                }

                if (assertionNode == null)
                {
                    return null;
                }

                foreach(var child in  assertionNode.ChildNodes)
                {
                    string s = "";
                }
            }

            return null;
        }
    }
}
