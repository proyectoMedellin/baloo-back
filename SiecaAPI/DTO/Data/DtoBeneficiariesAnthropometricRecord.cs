using SiecaAPI.Data.SQLImpl.Entities;
using System.ComponentModel.DataAnnotations;

namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiariesAnthropometricRecord
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public Guid TrainingCenterId { get; set; } = Guid.Empty;
        public string TrainingCenterCode { get; set; } = string.Empty;
        public string TrainingCenterName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
