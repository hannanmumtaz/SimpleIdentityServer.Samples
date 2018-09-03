using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApiProtection.MedicalPrescriptionsApi.DTOs.Responses
{
    [DataContract]
    public class MedicalRecordResponse
    {
        [DataMember(Name = "subject")]
        public string Subject { get; set; }
        [DataMember(Name = "uma_resource_id")]
        public string UmaResourceId { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
        [DataMember(Name = "prescriptions")]
        public IEnumerable<MedicalPrescriptionResponse> Prescriptions { get; set; }
    }
}
