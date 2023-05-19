using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; }
        public Boolean Enable { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Permission(Guid id, Guid organizationId, string name, string? description, string type, bool enable, string createdBy, DateTime? createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            Type = type;
            Enable = enable;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }
    }
}
