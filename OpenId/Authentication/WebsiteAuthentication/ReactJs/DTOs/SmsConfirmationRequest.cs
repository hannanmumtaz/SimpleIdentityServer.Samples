using System.Runtime.Serialization;

namespace WebsiteAuthentication.ReactJs.DTOs
{
    [DataContract]
    public class SmsConfirmationRequest
    {
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }
        [DataMember(Name = "confirmation_code")]
        public string ConfirmationCode { get; set; }
    }
}
