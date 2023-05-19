using Microsoft.EntityFrameworkCore;
using SiecaAPI.Controllers;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using System.Drawing.Printing;
using System.Linq;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoCampusSqlImpl : IDaoCampus
    {
        public async Task<DtoCampus> CreateAsync(DtoCampus campus)
        {
            using SqlContext context = new();
            OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == campus.OrganizationId).FirstAsync();

            TrainingCenterEntity tc = await context.TrainingCenters
                .Where(t => t.Id.Equals(campus.TrainingCenterId)).FirstAsync();

            if (org == null || tc == null)
                throw new NoDataFoundException("Organization or training center not found");

            CampusEntity newCampus = new()
            {
                OrganizationId = campus.OrganizationId,
                Organization = org,
                TrainingCenterId = campus.TrainingCenterId,
                TrainingCenter = tc,
                Code = campus.Code.ToUpper(),
                Name = campus.Name.ToUpper(),
                IntegrationCode = campus.IntegrationCode,
                Enabled = campus.Enabled,
                CreatedBy = campus.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            await context.Campuses.AddAsync(newCampus);
            await context.SaveChangesAsync();

            campus.Id = newCampus.Id;
            campus.OrganizationId = newCampus.OrganizationId;
            campus.OrganizationName = org.Name;
            campus.TrainingCenterId = newCampus.TrainingCenterId;
            campus.TrainingCenterName = tc.Name;

            return campus;
        }

        public async Task<DtoCampus> UpdateAsync(DtoCampus campus)
        {
            using SqlContext context = new();
            CampusEntity c = await context.Campuses
                .Where(c => c.Id.Equals(campus.Id)).FirstAsync();

            if (c != null)
            {
                if (!campus.TrainingCenterId.Equals(c.TrainingCenterId))
                {
                    TrainingCenterEntity newTc = await context.TrainingCenters
                        .Where(t => t.Id.Equals(campus.TrainingCenterId)).FirstAsync();
                    if(newTc == null) throw new NoDataFoundException("The campus dosent exists");

                    c.TrainingCenterId = campus.TrainingCenterId;
                    c.TrainingCenter = newTc;
                }

                c.Code = campus.Code.ToUpper();
                c.Name = campus.Name.ToUpper();
                c.IntegrationCode = campus.IntegrationCode;
                c.Enabled = campus.Enabled;
                c.ModifiedBy = campus.ModifiedBy;
                c.ModifiedOn = DateTime.UtcNow;
                await context.SaveChangesAsync();

                campus.OrganizationId = c.OrganizationId;
                campus.OrganizationName = c.Organization.Name;
                campus.TrainingCenterId = c.TrainingCenterId;
                campus.TrainingCenterName = c.TrainingCenter.Name;
            }
            else
            {
                throw new NoDataFoundException("The entity to update was not found");
            }
            return campus;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try {
                using SqlContext context = new();

                var campusToDelete = await context.Campuses.Where(tc => tc.Id.Equals(id)).FirstAsync();
                if (campusToDelete == null)
                {
                    throw new NoDataFoundException("The entity to delete was not found");
                }

                context.Remove(campusToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<DtoCampus>> GetAllAsync(int page, int pageSize, Guid? trainingCenterId,
            string? fCode, string? fName, bool? fEnabled)
        {
            List<DtoCampus> centers = new();

            using SqlContext context = new();
            int skipData = page > 0 ? (page - 1) * pageSize: 0;

            List<CampusEntity> campuses = await context.Campuses
                .Where( tc =>
                    (!trainingCenterId.HasValue || trainingCenterId.Value.Equals(tc.TrainingCenterId)) &&
                    (string.IsNullOrEmpty(fCode) || (!string.IsNullOrEmpty(fCode) && tc.Code.Contains(fCode.ToLower()))) &&
                    (string.IsNullOrEmpty(fName) || (!string.IsNullOrEmpty(fName) && tc.Name.Contains(fName.ToLower()))) &&
                    (!fEnabled.HasValue || (fEnabled.HasValue && tc.Enabled == fEnabled.Value))
                )
                .Include(tc => tc.TrainingCenter)
                .Skip(skipData).Take(pageSize).ToListAsync();

            foreach (CampusEntity tc in campuses)
            {
                centers.Add(new DtoCampus() { 
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    TrainingCenterId = tc.TrainingCenterId,
                    TrainingCenterCode = tc.TrainingCenter.Code,
                    TrainingCenterName = tc.TrainingCenter.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    Enabled = tc.Enabled
                });
            }
            return centers;
        }

        public async Task<List<DtoCampus>> GetActiveByTrainingCenterAsync(Guid trainingCenterId)
        {
            List<DtoCampus> centers = new();

            using SqlContext context = new();
            List<CampusEntity> campuses = await context.Campuses
                .Where(tc => tc.TrainingCenterId.Equals(trainingCenterId))
                .Include(tc => tc.TrainingCenter)
                .ToListAsync();

            foreach (CampusEntity tc in campuses)
            {
                centers.Add(new DtoCampus()
                {
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    TrainingCenterId = tc.TrainingCenterId,
                    TrainingCenterCode = tc.TrainingCenter.Code,
                    TrainingCenterName = tc.TrainingCenter.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    Enabled = tc.Enabled
                });
            }
            return centers;
        }

        public async Task<DtoCampus> GetByIdAsync(Guid id)
        {
            using SqlContext context = new();

            CampusEntity c = await context.Campuses
                .Where(c => c.Id.Equals(id))
                .Include(c => c.Organization)
                .Include(c => c.TrainingCenter)
                .FirstAsync();

            return new DtoCampus()
            {
                Id = c.Id,
                OrganizationId = c.OrganizationId,
                OrganizationName = c.Organization.Name,
                TrainingCenterId = c.TrainingCenterId,
                TrainingCenterCode = c.TrainingCenter.Code,
                TrainingCenterName = c.TrainingCenter.Name,
                Code = c.Code,
                Name = c.Name,
                IntegrationCode = !string.IsNullOrEmpty(c.IntegrationCode) ? c.IntegrationCode : String.Empty,
                Enabled = c.Enabled
            };
        }
    }
}
