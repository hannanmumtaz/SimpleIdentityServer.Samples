namespace ApiProtection.MedicalPrescriptionsApi.Parameters
{
    public class AddMedicalPrescriptionParameter
    {
        public string DoctorSubject { get; set; }
        public string PatientSubject { get; set; }
        public string Description { get; set; }
    }
}
