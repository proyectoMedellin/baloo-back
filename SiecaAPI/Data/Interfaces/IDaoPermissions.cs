using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoPermissions
    {
        public Task<DtoPermission> CreateAsync(DtoPermission permission);
        public Task<DtoPermission> UpdateAsync(DtoPermission permission);
        public Task<bool> DeleteAsync(DtoPermission permission);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<DtoPermission> GetByIdAsync(Guid id);
        public Task<DtoPermission> GetByPermissionNameAsync(string name);
        public Task<List<DtoPermission>> GetAllAsync();
        public Task<List<DtoPermission>> GetAllByUserNameAsync(string userName);
    }
}
