using ApiProtection.MedicalPrescriptionsApi.DTOs.Requests;
using ApiProtection.MedicalPrescriptionsApi.DTOs.Responses;
using ApiProtection.MedicalPrescriptionsApi.Models;
using ApiProtection.MedicalPrescriptionsApi.Parameters;
using System.Linq;

namespace ApiProtection.MedicalPrescriptionsApi.Extensions
{
    internal static class MappingExtensions
    {
        public static AddMedicalPrescriptionParameter ToParameter(this AddPrescriptionRequest addPrescriptionRequest)
        {
            return new AddMedicalPrescriptionParameter
            {
                Description = addPrescriptionRequest.Description,
                PatientSubject = addPrescriptionRequest.PatientSubject,
                DoctorSubject = addPrescriptionRequest.DoctorSubject
            };
        }

        public static MedicalPrescriptionResponse ToDto(this Prescription prescription)
        {
            return new MedicalPrescriptionResponse
            {
                CreateDatetime = prescription.CreateDatetime,
                DoctorSubject = prescription.DoctorSubject,
                Id = prescription.Id,
                Description = prescription.Description,
                UpdateDatetime = prescription.UpdateDatetime
            };
        }

        public static MedicalRecordResponse ToDto(this MedicalRecord medicalRecord)
        {
            return new MedicalRecordResponse
            {
                CreateDateTime = medicalRecord.CreateDateTime,
                Subject = medicalRecord.Subject,
                UmaResourceId = medicalRecord.UmaResourceId,
                UpdateDateTime = medicalRecord.UpdateDateTime,
                Prescriptions = medicalRecord.Prescriptions.Select(p => p.ToDto())
            };
        }
    }
}
