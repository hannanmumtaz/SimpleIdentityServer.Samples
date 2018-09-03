using System.Runtime.Serialization;

namespace ApiProtection.ReactJs.DTOs
{
    [DataContract]
    public class AddPrescriptionRequest
    {
        [DataMember(Name = "doctor_subject")]
        public string DoctorSubject { get; set; }
        [DataMember(Name = "patient_subject")]
        public string PatientSubject { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
