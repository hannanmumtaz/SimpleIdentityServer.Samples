using System.Runtime.Serialization;

namespace WebsiteAuthentication.ReactJs.DTOs
{
    [DataContract]
    public class SmsAuthenticateRequest
    {
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }
    }
}
