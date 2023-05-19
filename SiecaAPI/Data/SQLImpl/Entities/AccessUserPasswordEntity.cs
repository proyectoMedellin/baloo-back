using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("AccessUserPassword")]
    public class AccessUserPasswordEntity
    {
        [Key]
        public Guid AccessUserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public AccessUserPasswordEntity(Guid accessUserId, string password, string createdBy, DateTime createdOn, string? modifiedBy, DateTime? modifiedOn)
        {
            AccessUserId = accessUserId;
            Password = password;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            if (!string.IsNullOrEmpty(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                ModifiedOn = !modifiedOn.HasValue ? DateTime.UtcNow : modifiedOn.Value;
            }
        }

        public AccessUserPasswordEntity()
        {
            Password = String.Empty;
            CreatedBy = String.Empty;
            ModifiedBy = String.Empty;
        }
    }
}
