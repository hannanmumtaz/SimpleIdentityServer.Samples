using System;

namespace ApiProtection.MedicalPrescriptionsApi.Extensions
{
    internal static class MedicalPrescriptionDbContextExtensions
    {
        public static void EnsureSeedData(this MedicalPrescriptionDbContext context)
        {
            context.MedicalRecords.AddRange(new Models.MedicalRecord
            {
                Subject = "patient",
                UmaResourceId = "1",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow
            });
            context.Prescriptions.AddRange(new Models.Prescription
            {
                Id = Guid.NewGuid().ToString(),
                Description = "not valid",
                DoctorSubject = "doctor",
                MedicalRecordId = "patient",
                UpdateDatetime = DateTime.UtcNow,
                CreateDatetime = DateTime.UtcNow
            });
            try
            {
                context.SaveChanges();
            }
            catch
            {

            }
        }
    }
}
