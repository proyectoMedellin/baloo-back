namespace SiecaAPI.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; } = true;

        public Organization(string name)
        {
            Name = name;
        }

        public Organization(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
