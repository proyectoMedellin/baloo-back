using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiecaAPI.Data.SQLImpl.Entities
{
    [Table("Beneficiaries")]
    public class BeneficiariesEntity: GenericEntity
    {
        [Required]
        [ForeignKey("DocumentTypeId")]
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public virtual DocumentTypeEntity DocumentType { get; set; } = new();
        [Required]
        public string DocumentNumber { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        [Required]
        [ForeignKey("GenderId")]
        public Guid GenderId { get; set; } = Guid.Empty;
        public virtual BeneficiariesParametersEntity Gender { get; set; } = new();
        [Required]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        [Required]
        [ForeignKey("BirthCountryId")]
        public Guid BirthCountryId { get; set; } = Guid.Empty;
        public virtual CountryEntity BirthCountry { get; set; } = new();
        [Required]
        [ForeignKey("BirthDepartmentId")]
        public Guid BirthDepartmentId { get; set; } = Guid.Empty;
        public virtual DepartmentEntity BirthDepartment { get; set; } = new();
        [Required]
        [ForeignKey("BirthCityId")]
        public Guid BirthCityId { get; set; } = Guid.Empty;
        public virtual CityEntity BirthCity { get; set; } = new();
        [Required]
        [ForeignKey("RhId")]
        public Guid RhId { get; set; } = Guid.Empty;
        public virtual BeneficiariesParametersEntity Rh { get; set; } = new();
        [Required]
        [ForeignKey("BloodTypeId")]
        public Guid BloodTypeId { get; set; } = Guid.Empty;
        public virtual BeneficiariesParametersEntity BloodType { get; set; } = new();
        [Required]
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        [Required]
        [ForeignKey("AdressZoneId")]
        public Guid AdressZoneId { get; set; } = Guid.Empty;
        public virtual BeneficiariesParametersEntity AdressZone { get; set; } = new();
        public string Adress { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string AdressPhoneNumber { get; set; } = string.Empty;
        public string AdressObservations { get; set; } = string.Empty;
    }
}
