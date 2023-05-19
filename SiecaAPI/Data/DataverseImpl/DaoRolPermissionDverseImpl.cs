using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoRolPermissionDverseImpl : IDaoRolPermission
    {
        public Task<DtoRolPermission> CreateAsync(DtoRolPermission rolPermission)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoRolPermission rolPermission)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoRolPermission>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoRolPermission>> GetByOrgRolAsync(Guid orgId, Guid rolI)
        {
            throw new NotImplementedException();
        }

        public Task<DtoRolPermission> UpdateAsync(DtoRolPermission rolPermission)
        {
            throw new NotImplementedException();
        }
    }
}
