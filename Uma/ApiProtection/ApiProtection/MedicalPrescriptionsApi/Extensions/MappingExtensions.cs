using ApiProtection.MedicalPrescriptionsApi.DTOs.Requests;
using ApiProtection.MedicalPrescriptionsApi.DTOs.Responses;
using ApiProtection.MedicalPrescriptionsApi.Models;
using ApiProtection.MedicalPrescriptionsApi.Parameters;

namespace ApiProtection.MedicalPrescriptionsApi.Extensions
{
    internal static class MappingExtensions
    {
        public static AddMedicalPrescriptionParameter ToParameter(this AddPrescriptionRequest addPrescriptionRequest, string subject)
        {
            return new AddMedicalPrescriptionParameter
            {
                Description = addPrescriptionRequest.Description,
                PatientSubject = addPrescriptionRequest.PatientSubject,
                DoctorSubject = subject
            };
        }

        public static MedicalRecordResponse ToDto(this MedicalRecord medicalRecord)
        {
            return new MedicalRecordResponse
            {

            };
        }
    }
}
