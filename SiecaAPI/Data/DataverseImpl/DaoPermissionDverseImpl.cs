using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoPermissionDverseImpl : IDaoPermissions
    {
        public Task<DtoPermission> CreateAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoPermission>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoPermission>> GetAllByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<DtoPermission> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DtoPermission> GetByPermissionNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DtoPermission> UpdateAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }
    }
}
