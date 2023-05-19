using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("TrainingCenter")]
    public class TrainingCenterEntity: GenericEntity
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = String.Empty;
        public string? IntegrationCode { get; set; }
    }
}
