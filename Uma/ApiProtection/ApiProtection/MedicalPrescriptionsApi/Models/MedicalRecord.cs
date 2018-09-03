using System;
using System.Collections.Generic;

namespace ApiProtection.MedicalPrescriptionsApi.Models
{
    public class MedicalRecord
    {
        public string Subject { get; set; }
        public string UmaResourceId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
