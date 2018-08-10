using System.Runtime.Serialization;

namespace WebsiteProtection.ViewModels
{
    [DataContract]
    public class AuthenticateViewModel
    {
        [DataMember(Name = "login")]
        public string Login { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
