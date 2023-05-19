namespace SiecaAPI.DTO.Data
{
    public class GenericDataDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }
}
