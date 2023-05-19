namespace SiecaAPI.DTO.Requests
{
    public class DtoPermissionReq
    {
        //public Guid? OrganizationId { get; set; }
        public string? oldName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public Boolean? Enabled { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
