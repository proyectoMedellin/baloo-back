using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Models.Services
{
    public static class LocationServices
    {
        public static async Task<List<DtoCountry>> GetCountries()
        {
            return await DaoLocationFactory.GetDaoLocation().GetCountriesAsync();
        }

        public static async Task<List<DtoDepartment>> GetDeparmentsByCountry(Guid countryId)
        {
            return await DaoLocationFactory.GetDaoLocation().GetDepartmensByContryAsync(countryId);
        }

        public static async Task<List<DtoCity>> GetCitiesByDeparment(Guid departmentId)
        {
            return await DaoLocationFactory.GetDaoLocation().GetCitiesByDepartmentAsync(departmentId);
        }
    }
}
