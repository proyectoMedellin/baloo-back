namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiariesParameters
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid OrganizationId { get; set; } = Guid.Empty;
        public string ParamType { get; set; } = string.Empty;
        public string ParamCode { get; set; } = string.Empty;
        public string ParamValue { get; set; } = string.Empty;
        public int Position { get; set; } = 0;
    }
}
