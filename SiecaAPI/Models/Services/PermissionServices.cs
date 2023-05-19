using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class PermissionServices
    {

        public static async Task<List<DtoPermission>> GetAllPermissionAsync()
        {
            List<DtoPermission> Permissions = await DaoPermissionFactory
                .GetDaoPermission().GetAllAsync();
            return Permissions;
        }
        public static async Task<List<DtoPermission>> GetAllPermissionByUserNameAsync(string userName)
        {
            List<DtoPermission> Permissions = await DaoPermissionFactory
                .GetDaoPermission().GetAllByUserNameAsync(userName);
            return Permissions;
        }
    }
}
