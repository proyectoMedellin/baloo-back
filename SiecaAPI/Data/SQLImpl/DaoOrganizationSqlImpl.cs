using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoOrganizationSqlImpl : IDaoOrganizations
    {       
        public async Task<DtoOrganization> CreateAsync(DtoOrganization org)
        {                     

            OrganizationEntity newOrg = new OrganizationEntity(org.Name,
                true, org.CreatedBy, DateTime.UtcNow, null, null);

            using SqlContext context = new SqlContext();
            await context.Organizations.AddAsync(newOrg);
            await context.SaveChangesAsync();

            org.Id = newOrg.Id;
            return org;
                               
        }

        public async Task<bool> DeleteAsync(DtoOrganization org)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoOrganization>> GetAllAsync()
        {
            using SqlContext context = new SqlContext();
            List<OrganizationEntity> org = await context.Organizations.ToListAsync();
            List<DtoOrganization> dTOOrganizations = new();
            foreach(OrganizationEntity o in org)
            {
                dTOOrganizations.Add(new DtoOrganization(o.Id, o.Name, o.CreatedBy));
            }
            return dTOOrganizations;
            
        }
        public async Task<List<DtoOrganization>> GetAllEnabledAsync()
        {
            using SqlContext context = new SqlContext();
            List<OrganizationEntity> org = await context.Organizations.Where(o => o.Enabled).ToListAsync();
            List<DtoOrganization> dTOOrganizations = new();
            foreach (OrganizationEntity o in org)
            {
                dTOOrganizations.Add(new DtoOrganization(o.Id, o.Name, o.CreatedBy));

            }
            return dTOOrganizations;
        }

        public async Task<DtoOrganization> GetByIdAsync(Guid id)
        {
            using SqlContext context = new SqlContext();
            OrganizationEntity org = await context.Organizations.Where(o => o.Id.Equals(id)).FirstAsync();
            return new DtoOrganization(org.Id, org.Name, org.CreatedBy);
        }

        public async Task<DtoOrganization> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DtoOrganization> UpdateAsync(DtoOrganization org)
        {
            throw new NotImplementedException();
        }
    }
}
