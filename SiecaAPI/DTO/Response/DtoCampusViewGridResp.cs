namespace SiecaAPI.DTO.Response
{
    public class DtoCampusViewGridResp
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public string TrainingCenterCode { get; set; } = string.Empty;
        public string TrainingCenterName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
    }
}
