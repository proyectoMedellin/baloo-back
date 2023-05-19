using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Menu")]
    public class MenuEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        [Required]
        public string MenuLabel { get; set; }
        public string? Description { get; set; }
        public Guid? ParentMenuId { get; set; }
        public int Position { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Route { get; set; }
        [Required]
        [ForeignKey("PermissionId")]
        public Guid PermissionId { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public MenuEntity(OrganizationEntity organization, string menuLabel, string description, Guid? parentMenuId, int position, string externUrl, 
            string? route, Guid permissionId, bool enabled, string createdBy, DateTime createOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Organization = organization;
            MenuLabel = menuLabel;
            Description = description;
            ParentMenuId = parentMenuId;
            Position = position;
            ExternalUrl = externUrl;
            Route = route;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = String.Empty;
                ModifiedOn = DateTime.MinValue;
            }
        }
        public MenuEntity()
        {
            OrganizationId = Guid.Empty;
            Organization = new OrganizationEntity();
            MenuLabel = String.Empty;
            Description = String.Empty;
            ParentMenuId = Guid.Empty;
            Position = 0;
            ExternalUrl = String.Empty;
            Route = String.Empty;
            Enabled = true;
            CreatedBy = String.Empty;
            CreatedOn = DateTime.MinValue;
            ModifiedBy = String.Empty;
            ModifiedOn = DateTime.MinValue;
        }
        
    }
}
