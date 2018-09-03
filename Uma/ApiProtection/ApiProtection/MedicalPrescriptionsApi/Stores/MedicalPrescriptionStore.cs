using ApiProtection.MedicalPrescriptionsApi.DTOs.Responses;
using ApiProtection.MedicalPrescriptionsApi.Models;
using ApiProtection.MedicalPrescriptionsApi.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProtection.MedicalPrescriptionsApi.Stores
{
    public interface IMedicalPrescriptionStore
    {
        Task<bool> Add(AddMedicalPrescriptionParameter addMedicalPrescriptionParameter);
    }

    internal sealed class MedicalPrescriptionStore : IMedicalPrescriptionStore
    {
        private readonly MedicalPrescriptionDbContext _context;

        public MedicalPrescriptionStore(MedicalPrescriptionDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(AddMedicalPrescriptionParameter addMedicalPrescriptionParameter)
        {
            var record = new Prescription
            {
                Id = Guid.NewGuid().ToString(),
                MedicalRecordId = addMedicalPrescriptionParameter.PatientSubject,
                DoctorSubject = addMedicalPrescriptionParameter.DoctorSubject,
                Description = addMedicalPrescriptionParameter.Description,
                CreateDatetime = DateTime.UtcNow,
                UpdateDatetime = DateTime.UtcNow
            };
            await _context.Prescriptions.AddAsync(record).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public Task<IEnumerable<MedicalPrescriptionResponse>> GetAll(string subject)
        {
            throw new System.NotImplementedException();
        }
    }
}