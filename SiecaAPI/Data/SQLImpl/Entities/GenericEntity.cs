using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    public abstract class GenericEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; } = new();

        public bool Enabled { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
