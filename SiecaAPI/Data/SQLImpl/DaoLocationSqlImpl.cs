using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoLocationSqlImpl : IDaoLocation
    {
        public async Task<List<DtoCountry>> GetCountriesAsync()
        {
            List<DtoCountry> cResponse = new();
            using SqlContext context = new();
            List<CountryEntity> countries =  await context.Countries.ToListAsync();
            foreach (CountryEntity c in countries)
            {
                cResponse.Add(new DtoCountry()
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                });
            }

            return cResponse;
        }

        public async Task<List<DtoDepartment>> GetDepartmensByContryAsync(Guid countryId)
        {
            List<DtoDepartment> cResponse = new();
            using SqlContext context = new();
            List<DepartmentEntity> departments = await context.Departments
                .Where(d => d.CountryId.Equals(countryId)).ToListAsync();

            foreach (DepartmentEntity c in departments)
            {
                cResponse.Add(new DtoDepartment()
                {
                    Id = c.Id,
                    CountryId = c.CountryId,
                    Code = c.Code,
                    Name = c.Name
                });
            }

            return cResponse;
        }

        public async Task<List<DtoCity>> GetCitiesByDepartmentAsync(Guid departmentId)
        {
            List<DtoCity> cResponse = new();
            using SqlContext context = new();
            List<CityEntity> cities = await context.Cities
                .Where(d => d.DepartmentId.Equals(departmentId)).ToListAsync();

            foreach (CityEntity c in cities)
            {
                cResponse.Add(new DtoCity()
                {
                    Id = c.Id,
                    DepartmentId = c.DepartmentId,
                    Code = c.Code,
                    Name = c.Name
                });
            }

            return cResponse;
        }
    }
}
