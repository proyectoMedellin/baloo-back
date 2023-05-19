namespace SiecaAPI.DssPro.Models
{
    public class RawEmotionRecordDto
    {
        public string id { get; set; }
        public string personId { get; set; }
        public string channelName { get; set; }
        public string emotion { get; set; }
        public string captureTime { get; set; }
    }
}
