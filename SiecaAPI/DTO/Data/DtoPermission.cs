namespace SiecaAPI.DTO.Data
{
    public class DtoPermission
    {
        public Guid? Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; }
        public Boolean Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Boolean? IsUpdate { get; set; }
        public DtoPermission(Guid? id, Guid organizationId, string name, string? description, string type, bool enabled, string createdBy, DateTime? createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            Type = type;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }
        public DtoPermission(bool? isUpdate)
        {
            IsUpdate = isUpdate;
        }
    }
}
