using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoOrganizations
    {
        public Task<DtoOrganization> CreateAsync(DtoOrganization org);
        public Task<DtoOrganization> UpdateAsync(DtoOrganization org);
        public Task<bool> DeleteAsync(DtoOrganization org);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<DtoOrganization> GetByIdAsync(Guid id);
        public Task<DtoOrganization> GetByNameAsync(string name);
        public Task<List<DtoOrganization>> GetAllAsync();
        public Task<List<DtoOrganization>> GetAllEnabledAsync();
    }
}
