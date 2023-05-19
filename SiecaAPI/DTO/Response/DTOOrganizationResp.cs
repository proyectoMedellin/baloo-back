namespace SiecaAPI.DTO.Requests
{
    public class DtoOrganizationResp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DtoOrganizationResp(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
