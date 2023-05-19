namespace SiecaAPI.DTO.Requests
{
    public class DtoDevRoomGroupBeneficiariesCreateReq
    {
        public Guid DevelopmentRoomGroupByYearId { get; set; }
        public string AssignamentUser { get; set; } = string.Empty;
        public List<DtoBeneficiaryToAssign> Beneficiaries { get; set; } = new();
    }

    public class DtoBeneficiaryToAssign
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Names { get; set; } = string.Empty;
    }
}
