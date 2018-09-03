using ApiProtection.MedicalPrescriptionsApi.Mappings;
using ApiProtection.MedicalPrescriptionsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProtection.MedicalPrescriptionsApi
{
    public class MedicalPrescriptionDbContext : DbContext
    {
        public MedicalPrescriptionDbContext(DbContextOptions<MedicalPrescriptionDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddPrescriptionMappings();
            modelBuilder.AddMedicalRecordMappings();
        }
    }
}
