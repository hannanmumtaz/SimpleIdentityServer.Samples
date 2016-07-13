using Microsoft.AspNetCore.Builder;

namespace WsFederation
{
    public class WsFedAuthenticationOptions : CookieAuthenticationOptions
    {
        public string Realm { get; set; }
        public string SigningCertThumbprint { get; set; }
        public string EncryptionCertStoreName { get; set; }
        public bool IsPersistent { get; set; }
        public string IdPEndpoint { get; set; }
    }
}
