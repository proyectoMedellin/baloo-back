namespace SiecaAPI.DTO.Requests
{
    public class DtoRolCreateReq
    {
        public Guid? OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Boolean NewAccessUserDefaultRol { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public List<DtoRolPermissionReq> ListPermission { get; set; } = new List<DtoRolPermissionReq>();
    }
}
