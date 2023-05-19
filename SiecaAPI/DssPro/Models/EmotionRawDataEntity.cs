using Microsoft.Kiota.Abstractions;
using SiecaAPI.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.DssPro.DTO
{
    [Table("BeneficiaryRawEmotionsData")]
    public class EmotionRawDataEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string PersonId { get; set; } = string.Empty ;
        [Required]
        public string DahuaChannelName { get; set; } = string.Empty;
        [Required]
        public int EmotionId { get; set; } = PrConstants.EMOTION_NOT_FOUND;
        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
