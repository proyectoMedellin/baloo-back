using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("AccessUserRoles")]
    public class AccessUserRolEntity
    {
       

        [Key]
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; } = Guid.Empty;
        //public virtual OrganizationEntity Organization { get; set; } = new OrganizationEntity();

        public string? AccessUserExternalId { get; set; } = string.Empty;

        [ForeignKey("AccessUserId")]
        public Guid? AccessUserId { get; set; } = Guid.Empty;
        //public virtual AccessUserEntity AccessUser { get; set; } = new AccessUserEntity();

        [Required]
        [ForeignKey("RolId")]
        public Guid RolId { get; set; } = Guid.Empty;
        public virtual RolEntity? Rol { get; set; } 
        //public AccessUserRolEntity(Guid id, Guid organizationId, OrganizationEntity organization, string? accessUserExternalId, Guid? accessUserId, AccessUserEntity accessUser, Guid rolId, RolEntity rol)
        //{
        //    Id = id;
        //    OrganizationId = organizationId;
        //    Organization = organization;
        //    AccessUserExternalId = accessUserExternalId;
        //    AccessUserId = accessUserId;
        //    AccessUser = accessUser;
        //    RolId = rolId;
        //    Rol = rol;
        //}
        public AccessUserRolEntity( Guid organizationId, string? accessUserExternalId, Guid? accessUserId, Guid rolId)
        {
            OrganizationId = organizationId;
            AccessUserExternalId = accessUserExternalId;
            AccessUserId = accessUserId;
            RolId = rolId;
        }

        public AccessUserRolEntity()
        {
            OrganizationId = Guid.Empty;
            AccessUserExternalId = String.Empty;
            AccessUserId = Guid.Empty;
            //AccessUser = new AccessUserEntity();
            RolId = Guid.Empty;
           // Rol = new RolEntity();
        }
    }
}
