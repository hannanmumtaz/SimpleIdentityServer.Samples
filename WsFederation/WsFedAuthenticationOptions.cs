using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Xml;

namespace WsFederation
{
    public class WsFedAuthenticationOptions : RemoteAuthenticationOptions
    {
        #region Constructor

        public WsFedAuthenticationOptions()
        {
        }

        #endregion

        #region Properties

        public string Realm { get; set; }

        public string IdPEndpoint { get; set; }

        public string RedirectUrl { get; set; }

        public Func<XmlNodeList, List<Claim>> GetClaimsCallback { get; set; }

        #endregion
    }
}
