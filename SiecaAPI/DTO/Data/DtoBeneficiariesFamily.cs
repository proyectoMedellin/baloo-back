namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiariesFamily: GenericDataDto
    {
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        public Guid FamilyRelationId { get; set; } = Guid.Empty;
        public bool Attendant { get; set; } = false;
    }
}
