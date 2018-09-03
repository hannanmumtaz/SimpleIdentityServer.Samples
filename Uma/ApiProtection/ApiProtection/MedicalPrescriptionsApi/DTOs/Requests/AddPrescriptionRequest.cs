using System.Runtime.Serialization;

namespace ApiProtection.MedicalPrescriptionsApi.DTOs.Requests
{
    [DataContract]
    public class AddPrescriptionRequest
    {
        [DataMember(Name = "patient_subject")]
        public string PatientSubject { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
