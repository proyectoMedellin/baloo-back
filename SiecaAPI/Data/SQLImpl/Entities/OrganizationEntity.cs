using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Organization")]
    public class OrganizationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Enabled { get; set; } 

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } 

        [DataType(DataType.EmailAddress, ErrorMessage = "Formato incorrecto")]
        public string? ModifiedBy { get; set;}
	    public DateTime? ModifiedOn { get; set; }

        public OrganizationEntity(string name, bool enabled, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            Name = name;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = modifiedOn ?? DateTime.UtcNow ;
            }
        }

        public OrganizationEntity()
        {
            Name = String.Empty;
            Enabled = true;
            CreatedBy = String.Empty;
            CreatedOn = DateTime.UtcNow;
            ModifiedBy = String.Empty;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}
