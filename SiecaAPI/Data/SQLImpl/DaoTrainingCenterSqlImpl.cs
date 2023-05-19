using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using System.Drawing.Printing;
using System.Linq;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoTrainingCenterSqlImpl : IDaoTrainingCenter
    {
        public async Task<DtoTrainingCenter> CreateAsync(DtoTrainingCenter tcenter)
        {
            using SqlContext context = new();
            OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == tcenter.OrganizationId).FirstAsync();

            TrainingCenterEntity newCenter = new()
            {
                OrganizationId = tcenter.OrganizationId,
                Organization = org,
                Code = tcenter.Code.ToUpper(),
                Name = tcenter.Name.ToUpper(),
                IntegrationCode = tcenter.IntegrationCode,
                Enabled = tcenter.Enabled,
                CreatedBy = tcenter.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            await context.TrainingCenters.AddAsync(newCenter);
            await context.SaveChangesAsync();

            tcenter.Id = newCenter.Id;
            tcenter.OrganizationId = newCenter.OrganizationId;
            tcenter.OrganizationName = org.Name;

            return tcenter;
        }

        public async Task<DtoTrainingCenter> UpdateAsync(DtoTrainingCenter tcenter)
        {
            using SqlContext context = new();
            TrainingCenterEntity tc = await context.TrainingCenters
                .Where(tc => tc.Id.Equals(tcenter.Id)).FirstAsync();
            if (tc != null)
            {
                tc.Code = tcenter.Code.ToUpper();
                tc.Name = tcenter.Name.ToUpper();
                tc.IntegrationCode = tcenter.IntegrationCode;
                tc.Enabled = tcenter.Enabled;
                tc.ModifiedBy = tcenter.ModifiedBy;
                tc.ModifiedOn = DateTime.UtcNow;
                await context.SaveChangesAsync();

                tcenter.OrganizationId = tc.OrganizationId;
                tcenter.OrganizationName = tc.Organization.Name;
            }
            else
            {
                throw new NoDataFoundException("The entity to update was not found");
            }
            return tcenter;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try {
                using SqlContext context = new();

                var centerToDelete = await context.TrainingCenters.Where(tc => tc.Id.Equals(id)).FirstAsync();
                if (centerToDelete == null)
                {
                    throw new NoDataFoundException("The entity to delete was not found");
                }

                context.Remove(centerToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<DtoTrainingCenter>> GetAllEnabledAsync()
        {
            List<DtoTrainingCenter> centers = new();

            using SqlContext context = new();

            List<TrainingCenterEntity> tCenters = await context.TrainingCenters
                .Where(tc => tc.Enabled)
                .OrderBy(tc => tc.Code)
                .ToListAsync();

            foreach (TrainingCenterEntity tc in tCenters)
            {
                centers.Add(new DtoTrainingCenter()
                {
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    Enabled = tc.Enabled
                });
            }
            return centers;
        }

        public async Task<List<DtoTrainingCenter>> GetAllAsync(int page, int pageSize, string? fCode, 
            string? fName, bool? fEnabled)
        {
            List<DtoTrainingCenter> centers = new();

            using SqlContext context = new();
            int skipData = page > 0 ? (page - 1) * pageSize: 0;

            List<TrainingCenterEntity> tCenters = await context.TrainingCenters
                .Where( tc =>
                    (string.IsNullOrEmpty(fCode) || (!string.IsNullOrEmpty(fCode) && tc.Code.Contains(fCode.ToLower()))) &&
                    (string.IsNullOrEmpty(fName) || (!string.IsNullOrEmpty(fName) && tc.Name.Contains(fName.ToLower()))) &&
                    (!fEnabled.HasValue || (fEnabled.HasValue && tc.Enabled == fEnabled.Value))
                )
                .Skip(skipData).Take(pageSize).ToListAsync();

            foreach (TrainingCenterEntity tc in tCenters)
            {
                centers.Add(new DtoTrainingCenter() { 
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    Enabled = tc.Enabled
                });
            }
            return centers;
        }

        public async Task<DtoTrainingCenter> GetByIdAsync(Guid id)
        {
            using SqlContext context = new();

            TrainingCenterEntity tc = await context.TrainingCenters
                .Where(tc => tc.Id.Equals(id)).FirstAsync();

            return new DtoTrainingCenter()
            {
                Id = tc.Id,
                OrganizationId = tc.OrganizationId,
                OrganizationName = tc.Organization.Name,
                Code = tc.Code,
                Name = tc.Name,
                IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                Enabled = tc.Enabled
            };
        }
    }
}
