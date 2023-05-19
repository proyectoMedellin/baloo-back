using Microsoft.EntityFrameworkCore;
using SiecaAPI.Commons;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Models;
using System.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoRolPermissionSqlImpl : IDaoRolPermission
    {
        public async Task<DtoRolPermission> CreateAsync(DtoRolPermission rolPermission)
        {
            using SqlContext context = new SqlContext();
            if (rolPermission.OrganizationId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization ID");

            OrganizationEntity org = await context.Organizations
                .Where(o => o.Id == rolPermission.OrganizationId).FirstAsync();
            RolEntity rol = await context.Roles
                .Where(o => o.Id == rolPermission.RolId).FirstAsync();
            PermissionEntity per = await context.Permissions
                .Where(o => o.Id == rolPermission.PermissionId).FirstAsync();

            if (org != null && rol != null && per != null)
            {
                RolPermissionEntity newRolPermission = new(org,
                                           rol,
                                           per,
                                           rolPermission.AllowView,
                                           rolPermission.AllowUse,
                                           rolPermission.AllowCreate,
                                           rolPermission.AllowUpdate,
                                           rolPermission.AllowDelete);
                await context.RolePermissions.AddAsync(newRolPermission);
                await context.SaveChangesAsync();
                return rolPermission;
            }
            throw new NoDataFoundException("No organization found");
        }

        public async Task<bool> DeleteAsync(DtoRolPermission rolPermission)
        {
            using SqlContext context = new SqlContext();
            if (rolPermission.OrganizationId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization ID");

            OrganizationEntity org = await context.Organizations
                .Where(o => o.Id == rolPermission.OrganizationId).FirstAsync();
            RolEntity rol = await context.Roles
                .Where(o => o.Id == rolPermission.RolId).FirstAsync();
            PermissionEntity per = await context.Permissions
                .Where(o => o.Id == rolPermission.PermissionId).FirstAsync();

            RolPermissionEntity deleteRolPermission = await context.RolePermissions.
                Where(rp => rp.OrganizationId.Equals(org.Id) && rp.RolId.Equals(rol.Id) && rp.PermissionId.Equals(per.Id)).FirstAsync();

            if (org != null && rol != null && per != null)
            {
                context.RolePermissions.Remove(deleteRolPermission);
                await context.SaveChangesAsync();
                return true;
            }
            throw new NoDataFoundException("No organization or Rol or Permission no found ");
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoRolPermission>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoRolPermission>> GetByOrgRolAsync(Guid orgId, Guid rolId)
        {
            using SqlContext context = new SqlContext();
            if (orgId == Guid.Empty || rolId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization or rol ID");

            List<RolPermissionEntity> listeRolPermission = await context.RolePermissions.
                Where(rp => rp.OrganizationId.Equals(orgId) && rp.RolId.Equals(rolId)).ToListAsync();
            List<DtoRolPermission> dtoRolesPermission = new();
            foreach (RolPermissionEntity reg in listeRolPermission)
            {
                dtoRolesPermission.Add(new DtoRolPermission(reg.OrganizationId, reg.RolId, reg.PermissionId,
                    reg.AllowView, reg.AllowCreate, reg.AllowUpdate, reg.AllowDelete, reg.AllowUse));
            }
            return dtoRolesPermission;
        }

        public async Task<DtoRolPermission> UpdateAsync(DtoRolPermission rolPermission)
        {
            using SqlContext context = new SqlContext();
            if (rolPermission.OrganizationId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization ID");

            OrganizationEntity org = await context.Organizations
                .Where(o => o.Id == rolPermission.OrganizationId).FirstAsync();
            RolEntity rol = await context.Roles
                .Where(o => o.Id == rolPermission.RolId).FirstAsync();
            PermissionEntity per = await context.Permissions
                .Where(o => o.Id == rolPermission.PermissionId).FirstAsync();
            
            RolPermissionEntity UpdateRolPermission = await context.RolePermissions.
                Where(rp => rp.OrganizationId.Equals(org.Id) && rp.RolId.Equals(rol.Id) && rp.PermissionId.Equals(per.Id)).FirstAsync();

            if (org != null && rol != null && per != null)
            {
                UpdateRolPermission.Organization = org;
                UpdateRolPermission.Rol = rol;
                UpdateRolPermission.Permission = per;
                UpdateRolPermission.AllowView = rolPermission.AllowView;
                UpdateRolPermission.AllowUse = rolPermission.AllowUse;
                UpdateRolPermission.AllowCreate = rolPermission.AllowCreate;
                UpdateRolPermission.AllowUpdate = rolPermission.AllowUpdate;
                UpdateRolPermission.AllowDelete = rolPermission.AllowDelete;

                context.RolePermissions.Update(UpdateRolPermission);
                await context.SaveChangesAsync();
                return rolPermission;
            }
            throw new NoDataFoundException("No organization or Rol or Permission no found ");
        }
    }
}
