using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DevelopmentRoomGroupAgent")]
    public class DevelopmentRoomGroupAgentEntity
    {
        [Key, Column(Order = 0)]
        [ForeignKey("DevelopmentRoomGroupByYearId")]
        public Guid DevelopmentRoomGroupByYearId { get; set; } = Guid.Empty;
        public virtual DevelopmentRoomGroupByYearEntity DevelopmentRoomGroupByYear { get; set; } = new();

        [Key, Column(Order = 1)]
        [ForeignKey("AccessUserId")]
        public Guid AccessUserId { get; set; } = Guid.Empty;
        public virtual AccessUserEntity AccessUser { get; set; } = new();

    }
}
