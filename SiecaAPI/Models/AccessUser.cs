namespace SiecaAPI.Models
{
    public class AccessUser
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? OtherNames { get; set; }
        public string LastName { get; set; }
        public string? OtherLastName { get; set; }
        public bool RequirePaswordChange { get; set; }
        public bool Enabled { get; set; } = true;
        public Guid DocumentTypeId { get; set; }
        public string DocumentNo { get; set; }
        public Guid? TrainingCenterId { get; set; }
        public bool GlobalUser { get; set; }

        public AccessUser(string userName, string email, string firstName, string lastName)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public AccessUser(Guid id, Guid organizationId, string userName, string email, string firstName, string? otherNames, string lastName, string? otherLastName, Guid documentTypeId, string documentNo, Guid? trainingCenterId, bool globalUser)
        {
            Id = id;
            OrganizationId = organizationId;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            TrainingCenterId = trainingCenterId;
            GlobalUser = globalUser;
        }
    }
}
