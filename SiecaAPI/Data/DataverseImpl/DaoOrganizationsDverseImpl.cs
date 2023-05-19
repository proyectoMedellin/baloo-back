using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoOrganizationsDverseImpl : IDaoOrganizations
    {
        public Task<DtoOrganization> CreateAsync(DtoOrganization org)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoOrganization org)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoOrganization>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoOrganization>> GetAllEnabledAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DtoOrganization> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DtoOrganization> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DtoOrganization> UpdateAsync(DtoOrganization org)
        {
            throw new NotImplementedException();
        }
    }
}
