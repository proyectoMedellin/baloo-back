using SiecaAPI.Models;
using System.Reflection;

namespace SiecaAPI.DTO.Data
{
    public class DtoBeneficiaries: GenericDataDto
    {
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public string DocumentTypeName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string OtherNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OtherLastName { get; set; } = string.Empty;
        public Guid GenderId { get; set; } = Guid.Empty;
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public Guid BirthCountryId { get; set; } = Guid.Empty;
        public Guid BirthDepartmentId { get; set; } = Guid.Empty;
        public Guid BirthCityId { get; set; } = Guid.Empty;
        public Guid RhId { get; set; } = Guid.Empty;
        public Guid BloodTypeId { get; set; } = Guid.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public Guid AdressZoneId { get; set; } = Guid.Empty;
        public string Adress { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string AdressPhoneNumber { get; set; } = string.Empty;
        public string AdressObservations { get; set; } = string.Empty;
        public List<DtoBeneficiariesFamily> FamilyMembers { get; set; } = new();
        public Guid? TrainigCenterId { get; set; }
        public string? TrainigCenterCode { get; set; }
        public string? TrainigCenterName { get; set; }
        public Guid? CampusId { get; set; }
        public string? CampusCode { get; set; }
        public string? CampusName { get; set; }
        public Guid? DevelopmentRoomId { get; set; }
        public string? DevelopmentRoomCode { get; set; }
        public string? DevelopmentRoomName { get; set; }
    }
}
