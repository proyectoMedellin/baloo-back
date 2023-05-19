using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("BeneficiariesParameters")]
    public class BeneficiariesParametersEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public virtual OrganizationEntity Organization { get; set; } = new();
        [Required]
        public string ParamType { get; set; } = string.Empty;
        [Required]
        public string ParamCode { get; set; } = string.Empty;
        [Required]
        public string ParamValue { get; set; } = string.Empty;
        [Required]
        public int Position { get; set; } = 0;
    }
}
