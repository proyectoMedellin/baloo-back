using SiecaAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DevelopmentRoomGroupBeneficiary")]
    public class DevelopmentRoomGroupBeneficiaryEntity
    {
        [Key, Column(Order = 0)]
        [ForeignKey("DevelopmentRoomGroupByYearId")]
        public Guid DevelopmentRoomGroupByYearId { get; set; } = Guid.Empty;
        public virtual DevelopmentRoomGroupByYearEntity DevelopmentRoomGroupByYear { get; set; } = new();

        [Key, Column(Order = 1)]
        [ForeignKey("BeneficiaryId")]
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public virtual BeneficiariesEntity Beneficiary { get; set; } = new();

        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public virtual OrganizationEntity Organization { get; set; } = new();

        [ForeignKey("TrainingCenterId")]
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();

        [ForeignKey("CampusId")]
        public Guid CampusId { get; set; } = Guid.Empty;
        public virtual CampusEntity Campus { get; set; } = new();

        [ForeignKey("DevelopmentRoomId")]
        public Guid DevelopmentRoomId { get; set; } = Guid.Empty;
        public virtual DevelopmentRoomEntity DevelopmentRoom { get; set; } = new();
    }
}
