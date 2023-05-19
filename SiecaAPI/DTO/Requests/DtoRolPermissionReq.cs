namespace SiecaAPI.DTO.Requests
{
    public class DtoRolPermissionReq
    {
        public string PermissionName { get; set; }
        public bool AllowView { get; set; } = false;
        public bool AllowCreate { get; set; } = false;
        public bool AllowUpdate { get; set; } = false;
        public bool AllowDelete { get; set; } = false;
        public bool AllowUse { get; set; } = false;

        public DtoRolPermissionReq(string permissionName, bool allowView, bool allowCreate, bool allowUpdate, bool allowDelete, bool allowUse)
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
