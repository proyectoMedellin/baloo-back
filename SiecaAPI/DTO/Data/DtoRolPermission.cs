namespace SiecaAPI.DTO.Data
{
    public class DtoRolPermission
    {
        
        public Guid OrganizationId { get; set; }
        public Guid RolId { get; set; }
        public Guid PermissionId { get; set; }  
        public bool AllowView { get; set; } = false;
        public bool AllowCreate { get; set; } = false;
        public bool AllowUpdate { get; set; } = false;
        public bool AllowDelete { get; set; } = false;
        public bool AllowUse { get; set; } = false;
        public string? PermissionName { get; set; }
        public DtoRolPermission(Guid organizationId, Guid rolId, Guid permissionId,  bool allowView, bool allowCreate, bool allowUpdate, bool allowDelete, bool allowUse)
        {
            OrganizationId = organizationId;
            RolId = rolId;
            PermissionId = permissionId;
            AllowView = allowView;
            AllowCreate = allowCreate;
            AllowUpdate = allowUpdate;
            AllowDelete = allowDelete;
            AllowUse = allowUse;
        }
        public DtoRolPermission(string permissionName, bool allowView, bool allowCreate, bool allowUpdate, bool allowDelete, bool allowUse)
        {
            PermissionName = permissionName;
            AllowView = allowView;
            AllowCreate = allowCreate;
            AllowUpdate = allowUpdate;
            AllowDelete = allowDelete;
            AllowUse = allowUse;
        }
    }
}
