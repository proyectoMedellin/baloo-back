namespace SiecaAPI.DTO.Requests
{
    public class DtoAccessUserReq
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string? oldUserName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? OtherNames { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? OtherLastName { get; set; }
        public string? CreatedBy { get; set; }
        public string Phone { get; set; } = string.Empty;
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public string DocumentNo { get; set; } = string.Empty;
        public Guid? TrainingCenterId { get; set; } = Guid.Empty;
        public List<Guid>? CampusId { get; set; } = new List<Guid>();
        public List<Guid> RolsId { get; set; } = new();
        public bool GlobalUser { get; set; } = false;
    }
}
