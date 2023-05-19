using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoMenuSqlImpl : IDaoMenu
    {
        public Task<DtoMenu> CreateAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoMenu>> GetItems(string userName)
        {
          
            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();
            
            List<AccessUserRolEntity> userRoles = user != null && user.Id != Guid.Empty ?
                    await context.AccessUserRoles.Where(aur => aur.AccessUserId == user.Id).ToListAsync() :
                    await context.AccessUserRoles.Where(aur => aur.AccessUserExternalId == userName).ToListAsync();
            List<RolPermissionEntity> userPermission = new List<RolPermissionEntity>();
            foreach(var role in userRoles)
            {
                userPermission.AddRange(await context.RolePermissions.
                    Where(usp => usp.RolId == role.RolId && usp.OrganizationId == role.OrganizationId)
                    .ToListAsync());
            }

            List<MenuEntity> Menu = new List<MenuEntity>();
            foreach(var permission in userPermission)
            {
                Menu.AddRange(await context.Menu.
                Where(o => o.Enabled && o.PermissionId == permission.PermissionId)
                .ToListAsync());
            }
            List<DtoMenu> dtoMenu = new();
            foreach (MenuEntity item in Menu.OrderBy(m => m.Position))
            {
                dtoMenu.Add(new DtoMenu(
                    item.OrganizationId,
                    item.MenuLabel,
                    item.Description,
                    item.ParentMenuId,
                    item.Position,
                    item.ExternalUrl,
                    item.Route,
                    item.PermissionId,
                    item.CreatedBy
                    ));
            }
            return dtoMenu;
         
        }

        public Task<DtoMenu> UpdateAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }
    }
}
