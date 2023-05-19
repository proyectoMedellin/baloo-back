using Microsoft.EntityFrameworkCore;
using SiecaAPI.Commons;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Models;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoPermissionSqlImpl : IDaoPermissions
    {
        public async Task<DtoPermission> CreateAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoPermission>> GetAllAsync()
        {
         
             using SqlContext context = new SqlContext();
             List<PermissionEntity> permission = await context.Permissions.Where(o => o.Enabled).ToListAsync();
             List<DtoPermission> dtoPermission = new();
             foreach (PermissionEntity per in permission)
             {
                 dtoPermission.Add(new DtoPermission(
                     per.Id,
                     per.OrganizationId,
                     per.Name,
                     per.Description,
                     per.Type,
                     per.Enabled,
                     per.CreatedBy,
                     per.CreatedOn,
                     per.ModifiedBy,
                     per.ModifiedOn));
             }
             return dtoPermission;

        }

        public async Task<List<DtoPermission>> GetAllByUserNameAsync(string userName)
        {
            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();
            List<AccessUserRolEntity> userRoles = user != null && user.Id != Guid.Empty ?
                    await context.AccessUserRoles.Where(aur => aur.AccessUserId == user.Id).ToListAsync() :
                    await context.AccessUserRoles.Where(aur => aur.AccessUserExternalId == userName).ToListAsync();
            List<RolPermissionEntity> userPermission = new List<RolPermissionEntity>();
            foreach (var role in userRoles)
            {
                userPermission.AddRange(await context.RolePermissions.
                    Where(usp => usp.RolId == role.RolId)
                    .Where(usp => usp.OrganizationId == role.OrganizationId).ToListAsync());
            }
            List<PermissionEntity> permissions = new ();
            foreach (var permission in userPermission)
            {
                permissions.AddRange(await context.Permissions.
                Where(p => p.Id == permission.PermissionId).
                Where(p => p.Enabled).
                ToListAsync());
            }

            List<DtoPermission> dtoPermission = new();
            foreach (PermissionEntity per in permissions.Distinct().ToList())
            {
                dtoPermission.Add(new DtoPermission(
                    per.Id,
                    per.OrganizationId,
                    per.Name,
                    per.Description,
                    per.Type,
                    per.Enabled,
                    per.CreatedBy,
                    per.CreatedOn,
                    per.ModifiedBy,
                    per.ModifiedOn));
            }
            return dtoPermission;
        }

        public async Task<DtoPermission> GetByIdAsync(Guid id)
        {
            using SqlContext context = new SqlContext();
            PermissionEntity Permission = await context.Permissions.Where(o => o.Id == id).FirstOrDefaultAsync();
            DtoPermission dtoPermission = new DtoPermission(
                     Permission.Id,
                     Permission.OrganizationId,
                     Permission.Name,
                     Permission.Description,
                     Permission.Type,
                     Permission.Enabled,
                     Permission.CreatedBy,
                     Permission.CreatedOn,
                     Permission.ModifiedBy,
                     Permission.ModifiedOn);

            return dtoPermission;
        }

        public async Task<DtoPermission> GetByPermissionNameAsync(string name)
        {
            using SqlContext context = new SqlContext();
            PermissionEntity Permission = await context.Permissions.Where(o => o.Name == name).FirstOrDefaultAsync();
           DtoPermission dtoPermission = new DtoPermission(
                    Permission.Id,
                    Permission.OrganizationId,
                    Permission.Name,
                    Permission.Description,
                    Permission.Type,
                    Permission.Enabled,
                    Permission.CreatedBy,
                    Permission.CreatedOn,
                    Permission.ModifiedBy,
                    Permission.ModifiedOn);
 
            return dtoPermission;
        }

        public async Task<DtoPermission> UpdateAsync(DtoPermission permission)
        {
            throw new NotImplementedException();
        }
    }
}
