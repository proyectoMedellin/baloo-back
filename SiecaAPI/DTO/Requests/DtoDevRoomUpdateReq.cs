using System.Data.Entity.Core.Common;

namespace SiecaAPI.DTO.Requests
{
    public class DtoDevRoomUpdateReq
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid CampusId { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IntegrationCode { get; set; } = string.Empty;
        public string DahuaChannelCode { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
