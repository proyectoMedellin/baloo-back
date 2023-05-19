namespace SiecaAPI.DTO.Requests
{
    public class DTODocumentTypeReq
    {
        public Guid? Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public Boolean? Enabled { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
