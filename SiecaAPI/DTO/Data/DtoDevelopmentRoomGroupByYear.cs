namespace SiecaAPI.DTO.Data
{
    public class DtoDevelopmentRoomGroupByYear : GenericDataDto
    {
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public string TrainingCenterCode { get; set; } = string.Empty;
        public string TrainingCenterName { get; set; } = string.Empty;
        public Guid CampusId { get; set; } = Guid.Empty;
        public string CampusCode { get; set; } = string.Empty;
        public string CampusName { get; set; } = string.Empty;
        public Guid DevelopmentRoomId { get; set; } = Guid.Empty;
        public string DevelopmentRoomCode { get; set; } = string.Empty;
        public string DevelopmentRoomName { get; set; } = string.Empty;
        public int Year { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public List<string> Agents { get; set; } = new();
        public List<Guid> AgentsId { get; set; } = new();
    }
}
