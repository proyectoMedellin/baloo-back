namespace SiecaAPI.DTO.Data
{
    public class DtoUserRol
    {
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public Guid RolId { get; set; } = Guid.Empty;
        public string RolName { get; set; } = string.Empty;
        public List<DtoRolPermission> Permissions { get; set; } = new List<DtoRolPermission>();
    }
}
