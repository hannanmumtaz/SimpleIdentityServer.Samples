using ApiProtection.MedicalPrescriptionsApi.DTOs.Responses;
using ApiProtection.MedicalPrescriptionsApi.Extensions;
using ApiProtection.MedicalPrescriptionsApi.Models;
using ApiProtection.MedicalPrescriptionsApi.Parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiProtection.MedicalPrescriptionsApi.Stores
{
    public interface IMedicalRecordStore
    {
        Task<MedicalRecordResponse> Get(string subject);
        Task<bool> Add(AddMedicalRecordParameter addMedicalRecordParameter);
    }

    internal sealed class MedicalRecordStore : IMedicalRecordStore
    {
        private readonly MedicalPrescriptionDbContext _context;

        public MedicalRecordStore(MedicalPrescriptionDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecordResponse> Get(string subject)
        {
            var result = await _context.MedicalRecords.Include(m => m.Prescriptions).FirstOrDefaultAsync(r => r.Subject == subject).ConfigureAwait(false);
            return result == null ? null : result.ToDto();
        }

        public async Task<bool> Add(AddMedicalRecordParameter addMedicalRecordParameter)
        {
            var record = new MedicalRecord
            {
                Subject = addMedicalRecordParameter.Subject,
                UmaResourceId = addMedicalRecordParameter.UmaResourceId,
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow
            };
            await _context.MedicalRecords.AddAsync(record).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
