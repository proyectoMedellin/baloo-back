using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("AccessUser")]
    public class AccessUserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("OrganizationId")]
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? OtherNames { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? OtherLastName { get; set; }
        [Required]
        public bool RequirePaswordChange { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? Phone { get; set; }
        
        [ForeignKey("DocumentTypeId")]
        [Required]
        public Guid DocumentTypeId { get; set; }
        [Required]
        public string DocumentNo { get; set; }
        [ForeignKey("TraininCenterId")]
        public Guid? TrainingCenterId  { get; set; }
        public virtual TrainingCenterEntity? TrainingCenter  { get; set; }
        public bool GlobalUser { get; set; }
        public AccessUserEntity(OrganizationEntity organization, string userName, string email, string firstName, string? otherNames,
            string lastName, string? otherLastName, bool requirePaswordChange, bool enabled, string createBy, DateTime createdOn,
            string? modifiedBy, DateTime? modifiedOn, string? phone, Guid documentTypeId, string documentNo, 
            TrainingCenterEntity? trainingCenter, bool globalUser)
        {
            OrganizationId = organization.Id;
            Organization = organization;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            OtherNames = otherNames;
            LastName = lastName;
            OtherLastName = otherLastName;
            RequirePaswordChange = requirePaswordChange;
            Enabled = enabled;
            CreatedBy = createBy;
            CreatedOn = createdOn;
            Phone = phone;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = modifiedOn ?? DateTime.UtcNow;
            }
            DocumentTypeId = documentTypeId;
            DocumentNo = documentNo;
            if (trainingCenter != null)
            {
                TrainingCenterId = trainingCenter.Id;
                TrainingCenter = trainingCenter;
            }
            GlobalUser = globalUser;
        }

        public AccessUserEntity()
        {
            OrganizationId = Guid.Empty;
            Organization = new OrganizationEntity();
            UserName = String.Empty;
            Email = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            OtherNames = null;
            OtherLastName = null;
            CreatedBy = String.Empty;
            CreatedOn = DateTime.MinValue;
            ModifiedBy = String.Empty;
            Phone = String.Empty;
            DocumentTypeId = Guid.Empty;
            DocumentNo = string.Empty;
            TrainingCenterId = Guid.Empty;
            TrainingCenter = new TrainingCenterEntity();
        }
    }
}
