using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DevelopmentRoom")]
    public class DevelopmentRoomEntity : GenericEntity
    {
        [Required]
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();
        [Required]
        public Guid CampusId { get; set; } = Guid.Empty;    
        public virtual CampusEntity Campus { get; set; } = new();
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string IntegrationCode { get; set; } = string.Empty;
        public string DahuaChannelCode { get; set; } = string.Empty;
    }
}
