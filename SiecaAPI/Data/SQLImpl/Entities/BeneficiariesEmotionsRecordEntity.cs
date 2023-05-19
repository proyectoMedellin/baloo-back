using SiecaAPI.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("BeneficiaryEmotionsData")]
    public class BeneficiariesEmotionsRecordEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey(nameof(BeneficiaryId))]
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public virtual BeneficiariesEntity Beneficiary { get; set; } = new();
        [ForeignKey(nameof(DevelopmentRoomId))]
        public Guid DevelopmentRoomId { get; set; } = Guid.Empty;
        public virtual DevelopmentRoomEntity DevelopmentRoom { get; set; } = new();
        public string IntegrationId { get; set; } = string.Empty;
        public int EmotionId { get; set; } = PrConstants.EMOTION_NOT_FOUND;
        public DateTime CreatedOn { get; set; }
    }
}
