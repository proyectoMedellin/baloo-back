using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("RolPermission")]
    public class RolPermissionEntity
    {
       
        [Key, Column(Order = 0)]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public virtual OrganizationEntity Organization { get; set; } = new OrganizationEntity();

        [Key, Column(Order = 1)]
        [ForeignKey("RolId")]
        public Guid RolId { get; set; } = Guid.Empty;
        public virtual RolEntity Rol { get; set; } = new RolEntity();

        [Key, Column(Order = 2)]
        [ForeignKey("PermissionId")]
        public Guid PermissionId { get; set; } = Guid.Empty;
        public virtual PermissionEntity Permission { get; set; } = new PermissionEntity();

        [Required]
        public bool AllowView { get; set; } = false;

        [Required]
        public bool AllowUse { get; set; } = false;

        [Required]
        public bool AllowCreate { get; set; } = false;

        [Required]
        public bool AllowUpdate { get; set; } = false;

        [Required]
        public bool AllowDelete { get; set; } = false;
        public RolPermissionEntity(OrganizationEntity organization, RolEntity rol, PermissionEntity permission, bool allowView, bool allowUse, bool allowCreate, bool allowUpdate, bool allowDelete)
        {
            OrganizationId = organization.Id;
            Organization = organization;
            RolId = rol.Id;
            Rol = rol;
            PermissionId = permission.Id;
            Permission = permission;
            AllowView = allowView;
            AllowUse = allowUse;
            AllowCreate = allowCreate;
            AllowUpdate = allowUpdate;
            AllowDelete = allowDelete;
        }
        public RolPermissionEntity()
        {
        }
    }
}
