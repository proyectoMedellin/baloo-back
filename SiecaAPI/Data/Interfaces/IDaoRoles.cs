using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoRoles
    {
        public Task<DtoRol> CreateAsync(DtoRol rol);
        public Task<DtoRol> UpdateAsync(DtoRol rol);
        public Task<bool> DeleteAsync(DtoRol rol);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoRol>> GetAllAsync();
        public Task<DtoRol> GetByNameAsync(string Name);
        public Task<DtoRol> GetByIdAsync(Guid id);
        public Task<bool> GetUserIsAdmin(string UserName);
    }
}
