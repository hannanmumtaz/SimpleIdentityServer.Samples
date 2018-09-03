using ApiProtection.MedicalPrescriptionsApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiProtection.MedicalPrescriptionsApi.Mappings
{
    internal static class MedicalRecordMapping
    {
        public static ModelBuilder AddMedicalRecordMappings(this ModelBuilder modelBuilder)
        {
            if(modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<MedicalRecord>()
                .ToTable("medicalRecords")
                .HasKey(p => p.Subject);
            return modelBuilder;
        }
    }
}
