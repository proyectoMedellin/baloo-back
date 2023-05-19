namespace SiecaAPI.DTO.Response
{
    public class DtoTrainingCenterViewGridResp
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
    }
}
