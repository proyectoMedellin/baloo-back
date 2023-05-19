namespace SiecaAPI.DTO.Data
{
    public class DtoCampus: GenericDataDto
    {

        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public string TrainingCenterCode { get; set; } = String.Empty;
        public string TrainingCenterName { get; set; } = String.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IntegrationCode { get; set; } = string.Empty;
    }
}
