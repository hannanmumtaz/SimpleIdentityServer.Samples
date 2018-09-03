using System;
using System.Runtime.Serialization;

namespace ApiProtection.MedicalPrescriptionsApi.DTOs.Responses
{
    [DataContract]
    public class MedicalPrescriptionResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "doctor_subject")]
        public string DoctorSubject { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDatetime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDatetime { get; set; }
    }
}
