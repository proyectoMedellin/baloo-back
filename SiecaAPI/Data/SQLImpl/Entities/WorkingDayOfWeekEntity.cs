using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("WorkingDayOfWeek")]
    public class WorkingDayOfWeekEntity
    {
        [Key]
        public int Year { get; set; }
        [Required]
        public bool Monday { get; set; } = false;
        [Required]
        public bool Tuesday { get; set; } = false;
        [Required]
        public bool Wednesday { get; set; } = false;
        [Required]
        public bool Thursday { get; set; } = false;
        [Required]
        public bool Friday { get; set; } = false;
        [Required]
        public bool Saturday { get; set; } = false;
        [Required]
        public bool Sunday { get; set; } = false;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }
}
