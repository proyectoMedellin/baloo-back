namespace SiecaAPI.DTO.Data
{
    public class DTODocumentType
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Boolean Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public DTODocumentType(Guid? id, Guid organizationId, string code, string name, bool enabled, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Id = id;
            OrganizationId = organizationId;
            Code = code;
            Name = name;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }
        
    }
}
