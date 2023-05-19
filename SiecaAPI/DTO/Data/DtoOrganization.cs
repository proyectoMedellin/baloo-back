namespace SiecaAPI.DTO.Data
{
    public class DtoOrganization
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }    
        public string CreatedBy { get; set; }

        public DtoOrganization(string name, string createdBy)
        {
            Name = name;
            CreatedBy = createdBy;
        }
        public DtoOrganization(Guid id, string name, string createBy)
        {
            Id = id;
            Name = name;
            CreatedBy = createBy;
        }
    }
}
