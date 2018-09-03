using ApiProtection.MedicalPrescriptionsApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiProtection.MedicalPrescriptionsApi.Mappings
{
    internal static class PrescriptionMapping
    {
        public static ModelBuilder AddPrescriptionMappings(this ModelBuilder modelBuilder)
        {
            if(modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Prescription>()
                .ToTable("prescriptions")
                .HasKey(p => p.Id);
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.MedicalRecord)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId);
            return modelBuilder;
        }
    }
}
