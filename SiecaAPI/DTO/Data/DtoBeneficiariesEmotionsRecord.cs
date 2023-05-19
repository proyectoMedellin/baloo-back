using SiecaAPI.Commons;

namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiariesEmotionsRecord
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public Guid DevelopmentRoomId { get; set; } = Guid.Empty;
        public string IntegrationId { get; set; } = string.Empty;
        public int EmotionId { get; set; } = PrConstants.EMOTION_NORMAL;
        public string EmotionName { get; set; } = PrConstants.GetEmotionName(PrConstants.EMOTION_NORMAL);
        public DateTime CreatedOn { get; set; }
    }
}
