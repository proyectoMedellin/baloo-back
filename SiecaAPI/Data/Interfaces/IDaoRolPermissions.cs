using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoRolPermission
    {
        public Task<DtoRolPermission> CreateAsync(DtoRolPermission rolPermission);
        public Task<DtoRolPermission> UpdateAsync(DtoRolPermission rolPermission);
        public Task<bool> DeleteAsync(DtoRolPermission rolPermission);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoRolPermission>> GetAllAsync();
        public Task<List<DtoRolPermission>> GetByOrgRolAsync(Guid orgId, Guid rolId);
    }
}
