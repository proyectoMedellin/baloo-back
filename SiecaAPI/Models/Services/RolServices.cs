using Azure.Core;
using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class RolServices
    {

        public static async Task<List<DtoRol>> GetAllRolAsync()
        {
            
            List<DtoRol> roles = await DaoRolFactory
                .GetDaoRoles().GetAllAsync();
            return roles;
        }
        public static async Task<Rol> CreateRolAsync(string name, string description, bool newAccessUserDefaultRol, string createdBy, 
            List<DtoRolPermissionReq> listPermission)
        {
            //Crea ROL
            Organization org = await OrganizationServices.GetActiveOrganization();
            DtoRol rol = new(Guid.Empty, org.Id, name, description, newAccessUserDefaultRol, true, createdBy, null, null, null);
            rol.OrganizationId = org.Id;
            rol =  await DaoRolFactory.GetDaoRoles().CreateAsync(rol);

            if (rol.Id == Guid.Empty) throw new InvalidOperationException("El rol no fue creado exitosamente");
            //Asigna Permisos
            List<DtoRolPermission> permissionByRol = new();
            foreach (DtoRolPermissionReq item in listPermission)
            {
                DtoPermission permission = await DaoPermissionFactory.GetDaoPermission().GetByPermissionNameAsync(item.PermissionName);
                if (!permission.Id.HasValue) throw new InvalidOperationException("El permiso a asignar no es valido");

                permissionByRol.Add( await DaoRolPermissionFactory.GetDaoRolPermission().CreateAsync(
                    new DtoRolPermission(org.Id, rol.Id, permission.Id.Value, item.AllowView, 
                        item.AllowCreate, item.AllowUpdate, item.AllowDelete, item.AllowUse)
                    ));
            }

            return new Rol(rol.Id, rol.OrganizationId, rol.Name, rol.Description, rol.NewAccessUserDefaultRol, rol.Enabled, 
                rol.CreatedBy, rol.CreatedOn, null, null);
        }
        public static async Task<DtoRol> UpdateRolAsync(Guid rolId, string name, string description, bool newAccessUserDefaultRol, 
            bool enabled, string modifiedBy, List<DtoRolPermissionReq> listPermission)
        {
            //Update ROl
            DtoRol rol = await  DaoRolFactory.GetDaoRoles().GetByIdAsync(rolId);
            if (rol == null) throw new InvalidOperationException("No existe el rol a actualizar");

            rol.Name = name;
            rol.Description = description;
            rol.NewAccessUserDefaultRol = newAccessUserDefaultRol;
            rol.Enabled = enabled;
            rol.ModifiedBy = modifiedBy;
            rol.ModifiedOn = DateTime.UtcNow;
            
            rol = await DaoRolFactory.GetDaoRoles().UpdateAsync(rol);
            
            if (rol.Id == Guid.Empty) throw new InvalidOperationException("El rol fue actualizado exitosamente");
            
            //Update Permisos
            List <DtoRolPermission> permissionByRol = new();
            foreach (DtoRolPermissionReq item in listPermission)
            {
                DtoPermission permission = await DaoPermissionFactory.GetDaoPermission().GetByPermissionNameAsync(item.PermissionName);
                if (!permission.Id.HasValue) throw new InvalidOperationException("El permiso a asignar no es valido");

                permissionByRol.Add(await DaoRolPermissionFactory.GetDaoRolPermission().UpdateAsync(
                    new DtoRolPermission(rol.OrganizationId, rol.Id, permission.Id.Value, item.AllowView,
                        item.AllowCreate, item.AllowUpdate, item.AllowDelete, item.AllowUse)
                    ));
            }
            ////Lista RolPermisos actuales
            //List<DtoRolPermission> OldpermissionByRol = await DaoRolPermissionFactory.GetDaoRolPermission().GetByOrgRolAsync(rol.OrganizationId, rol.Id);
            ////Delete registros que no estan en lista
            //List<DtoRolPermission> deletepermissionByRol = OldpermissionByRol.Except(permissionByRol).ToList();
            //foreach (DtoRolPermission item in deletepermissionByRol)
            //{
            //    bool deletePrmission = await DaoRolPermissionFactory.GetDaoRolPermission().DeleteAsync(item);
            //    if (!deletePrmission) throw new InvalidOperationException("El no se Elimino");
            //}
            return rol;
        }
        public static async Task<bool> GetUserIsAdmin(string userName)
        {

            bool isAdmin = await DaoRolFactory
                .GetDaoRoles().GetUserIsAdmin(userName);
            return isAdmin;
        }
    }
}
