using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoLocation
    {
        public Task<List<DtoCountry>> GetCountriesAsync();
        public Task<List<DtoDepartment>> GetDepartmensByContryAsync(Guid countryId);
        public Task<List<DtoCity>> GetCitiesByDepartmentAsync(Guid departmentId);

    }
}
