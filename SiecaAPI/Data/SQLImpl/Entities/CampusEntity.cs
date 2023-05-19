using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Campus")]
    public class CampusEntity : GenericEntity
    {
        [Required]
        public Guid TrainingCenterId { get; set; }
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = String.Empty;
        public string? IntegrationCode { get; set; }
    }
}
