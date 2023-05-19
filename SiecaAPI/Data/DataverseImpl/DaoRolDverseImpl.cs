using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoRolDverseImpl : IDaoRoles
    {
        public Task<DtoRol> CreateAsync(DtoRol rol)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoRol rol)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoRol>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DtoRol> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DtoRol> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetUserIsAdmin(string UserName)
        {
            throw new NotImplementedException();
        }

        public Task<DtoRol> UpdateAsync(DtoRol rol)
        {
            throw new NotImplementedException();
        }
    }
}
