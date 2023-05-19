using SiecaAPI.Data.SQLImpl.Entities;

namespace SiecaAPI.Models
{
    public class DocumentType
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DocumentType(Guid id, Guid organizationId, string code, string name, bool enable, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Id = id;
            OrganizationId = organizationId;
            Code = code;
            Name = name;
            Enable = enable;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

    }
}
