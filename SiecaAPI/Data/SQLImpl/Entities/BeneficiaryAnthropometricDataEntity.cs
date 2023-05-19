using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("BeneficiaryAnthropometricData")]
    public class BeneficiaryAnthropometricDataEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;

        public Guid BeneficiaryId { get; set; }

        public Guid TrainingCenterId { get; set; }

        public string? Comment { get; set; }

        public decimal Weight { get; set; }

        public decimal Height { get; set; }

        public decimal Bmi { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [ForeignKey(nameof(BeneficiaryId))]
        public virtual BeneficiariesEntity Beneficiaries { get; set; } = new();

        [ForeignKey(nameof(TrainingCenterId))]
        public virtual TrainingCenterEntity TrainingCenter { get; set; } = new();
    }
}
