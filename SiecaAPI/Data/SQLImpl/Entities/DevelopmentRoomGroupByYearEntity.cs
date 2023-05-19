using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DevelopmentRoomGroupByYear")]
    public class DevelopmentRoomGroupByYearEntity : GenericEntity
    {
        [Required]
        [ForeignKey("DevelopmentRoomId")]
        public Guid DevelopmentRoomId { get; set; }
        public virtual DevelopmentRoomEntity DevelopmentRoom { get; set; } = new();
        public int Year { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
