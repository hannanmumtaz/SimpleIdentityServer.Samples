using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using WsFederation.Messages;

namespace WsFederation
{
    internal class WsFedAuthenticationHandler : AuthenticationHandler<WsFedAuthenticationOptions>
    {
        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            var returnUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (!string.IsNullOrWhiteSpace(Options.RedirectUrl))
            {
                returnUrl = Options.RedirectUrl;
            }

            var signInResponse = new SignInRequestMessage(new Uri(Options.IdPEndpoint), Options.Realm, returnUrl);
            Response.Redirect(signInResponse.RequestUrl);
            return true;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult result = null;
            try
            {
                if (string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
              && !string.IsNullOrWhiteSpace(Request.ContentType)
              // May have media/type; charset=utf-8, allow partial match.
              && Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
              && Request.Body.CanRead)
                {
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
                    foreach (var tuple in form)
                    {
                        collection.Add(tuple.Key, tuple.Value);
                    }

                    var uri = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                    var wsFederationMessage = WSFederationMessage.CreateFromNameValueCollection(uri, collection);
                    var xml = wsFederationMessage.GetParameter("wresult");

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
                        return AuthenticateResult.Skip();
                    }

                    List<Claim> claims = null;
                    if (Options.GetClaimsCallback != null)
                    {
                        claims = Options.GetClaimsCallback(assertionNode.ChildNodes);
                    }
                    else
                    {
                        claims = new List<Claim>();
                        foreach (XmlNode child in assertionNode.ChildNodes)
                        {
                            if (child.Name == "saml2:Subject" || child.Name == "saml:Subject")
                            {
                                claims.Add(new Claim("sub", child.InnerText));
                            }

                            if (child.Name == "saml2:AttributeStatement"
                                || child.Name == "saml:AttributeStatement")
                            {
                                foreach (XmlNode attribute in child.ChildNodes)
                                {
                                    var id = string.Empty;
                                    foreach (XmlAttribute metadata in attribute.Attributes)
                                    {
                                        if (metadata.Name == "Name")
                                        {
                                            id = metadata.Value;
                                        }
                                    }

                                    claims.Add(new Claim(id, attribute.InnerText));
                                }
                            }
                        }
                    }
                    
                    var claimsIdentity = new ClaimsIdentity(claims);
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    var ticket = new AuthenticationTicket(principal, 
                        new AuthenticationProperties(), 
                        Options.AuthenticationScheme);
                    result = AuthenticateResult.Success(ticket);
                    if (PriorHandler != null)
                    {
                        var signInContext = new SignInContext(Options.SignInScheme,
                            principal,
                            new Dictionary<string, string>());
                        await PriorHandler.SignInAsync(signInContext);
                    }
                }
            }
            catch(Exception exception)
            {
                result = AuthenticateResult.Fail(exception);
            }
            
            return result;
        }
    }
}
