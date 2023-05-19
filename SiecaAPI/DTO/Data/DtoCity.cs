namespace SiecaAPI.DTO.Data
{
    public class DtoCity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid DepartmentId { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
