using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Rol")]
    public class RolEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.Empty;
        
        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool NewAccessUserDefaultRol { get; set; } = false;
        public bool Enabled { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public RolEntity(OrganizationEntity organization, string name, string? description, bool newAccessUserDefaultRol, bool enabled, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            OrganizationId = organization.Id;
            Organization = organization;
            Name = name;
            Description = description;
            NewAccessUserDefaultRol = newAccessUserDefaultRol;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = !modifiedOn.HasValue ? DateTime.UtcNow : modifiedOn.Value;
            }
        }
        public RolEntity()
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
