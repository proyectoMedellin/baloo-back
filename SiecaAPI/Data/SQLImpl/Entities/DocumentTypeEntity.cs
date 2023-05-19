using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("DocumentTypes")]
    public class DocumentTypeEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual OrganizationEntity Organization { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Boolean Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public DocumentTypeEntity( OrganizationEntity organization, string code, string name, bool enabled, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            
            OrganizationId = organization.Id;
            Organization = organization;
            Code = code;
            Name = name;
            Enabled = enabled;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = !modifiedOn.HasValue ? DateTime.UtcNow : modifiedOn.Value;
            }
        }
        public DocumentTypeEntity()
        {
            OrganizationId = Guid.Empty;
            Organization = new OrganizationEntity();
            Code = String.Empty;
            Name = String.Empty;
            CreatedBy = String.Empty;
            CreatedOn = DateTime.MinValue;
            ModifiedBy = String.Empty;
        }
    }
}
