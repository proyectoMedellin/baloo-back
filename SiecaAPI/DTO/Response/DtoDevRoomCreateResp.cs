using System.Data.Entity.Core.Common;

namespace SiecaAPI.DTO.Response
{
    public class DtoDevRoomCreateResp
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public Guid CampusId { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IntegrationCode { get; set; } = string.Empty;
        public string DahuaChannelCode { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
