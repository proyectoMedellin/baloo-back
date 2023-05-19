using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("City")]
    public class CityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        [ForeignKey("DepartmentId")]
        public Guid DepartmentId { get; set; } = Guid.Empty;
        public virtual DepartmentEntity Department { get; set; } = new();
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
