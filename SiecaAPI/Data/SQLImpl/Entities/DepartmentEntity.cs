using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Department")]
    public class DepartmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        [ForeignKey("CountryId")]
        public Guid CountryId { get; set; } = Guid.Empty;
        public virtual CountryEntity Country { get; set; } = new();
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
