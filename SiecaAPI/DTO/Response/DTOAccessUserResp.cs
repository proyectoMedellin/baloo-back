using SiecaAPI.Models;

namespace SiecaAPI.DTO.Response
{
    public class DtoAccessUserResp
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? OtherNames { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? OtherLastName { get; set; }
        public string? CreatedBy { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentNo { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid? TrainingCenterId { get; set; } = Guid.Empty;
        public List<Guid> CampusId { get; set; } = new();
        public List<Guid> RolsId { get; set; } = new();
        public bool GlobalUser { get; set; } = false;
    }
}
