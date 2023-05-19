namespace SiecaAPI.DTO.Data
{
    public class DtoDepartment
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid CountryId { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
