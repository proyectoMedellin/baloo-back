using SiecaAPI.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Permission")]
    public class PermissionEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public virtual OrganizationEntity Organization { get; set; } = new OrganizationEntity();

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required]
        public string Type { get; set; } = PrConstants.PERMISSION_TYPE_MENU;

        [Required]
        public bool Enabled { get; set; } = false;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public PermissionEntity(OrganizationEntity organization, string name, string? description, string type, bool enabled, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            OrganizationId = organization.Id;
            Organization = organization;
            Name = name;
            Description = description;
            Type = type;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = !modifiedOn.HasValue ? DateTime.UtcNow : modifiedOn.Value;
            }
        }
        public PermissionEntity()
        {
            OrganizationId = Guid.Empty;
            Organization = new OrganizationEntity();
            Name = String.Empty;
            Description = String.Empty;
            CreatedBy = String.Empty;
            CreatedOn = DateTime.MinValue;
            ModifiedBy = String.Empty;
        }
    }
}
