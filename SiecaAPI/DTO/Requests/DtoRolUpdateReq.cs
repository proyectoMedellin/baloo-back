namespace SiecaAPI.DTO.Requests
{
    public class DtoRolUpdateReq
    {
        public Guid RolId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Boolean NewAccessUserDefaultRol { get; set; } = false;
        public Boolean Enabled { get; set; } = false;
        public string ModifiedBy { get; set; } = string.Empty;
        public List<DtoRolPermissionReq>? ListPermission { get; set; }
    }
}
