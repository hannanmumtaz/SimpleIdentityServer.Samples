using System;

namespace ApiProtection.MedicalPrescriptionsApi.Extensions
{
    internal static class MedicalPrescriptionDbContextExtensions
    {
        public static void EnsureSeedData(this MedicalPrescriptionDbContext context)
        {
            context.MedicalRecords.AddRange(new[]
            {
                new Models.MedicalRecord
                {
                    Subject = "patient1",
                    UmaResourceId = "1",
                    UpdateDateTime = DateTime.UtcNow,
                    CreateDateTime = DateTime.UtcNow
                },
                new Models.MedicalRecord
                {
                    Subject = "patient2",
                    UmaResourceId = "2",
                    UpdateDateTime = DateTime.UtcNow,
                    CreateDateTime = DateTime.UtcNow
                }
            });
            context.Prescriptions.AddRange(new Models.Prescription
            {
                Id = Guid.NewGuid().ToString(),
                Description = "not valid",
                DoctorSubject = "doctor",
                MedicalRecordId = "patient1",
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
