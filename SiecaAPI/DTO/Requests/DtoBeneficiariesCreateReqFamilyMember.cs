namespace SiecaAPI.DTO.Requests
{
    public class DtoBeneficiariesCreateReqFamilyMember
    {
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        public Guid FamilyRelation { get; set; } = Guid.Empty;
        public bool Attendant { get; set; } = false;
        public bool Enabled { get; set; } = false;
    }
}
