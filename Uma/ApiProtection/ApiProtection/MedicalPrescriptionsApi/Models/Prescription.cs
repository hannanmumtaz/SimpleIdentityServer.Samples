using System;

namespace ApiProtection.MedicalPrescriptionsApi.Models
{
    public class Prescription
    {
        public string Id { get; set; }
        public string MedicalRecordId { get; set; }
        public string DoctorSubject { get; set; }
        public string Description { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }
    }
}
