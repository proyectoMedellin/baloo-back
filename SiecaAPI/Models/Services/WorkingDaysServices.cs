using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Models.Services
{
    public static class WorkingDaysServices
    {
        public static async Task<bool> ConfigureAsync(DtoWorkingDays wd)
        {
            return await DaoWorkingDaysFactory.GetDaoWorkingDay().ConfigureAsync(wd);
        }

        public static async Task<bool> DeleteByYearAsync(int year)
        {
            return await DaoWorkingDaysFactory.GetDaoWorkingDay().DeleteByYearAsync(year);
        }

        public static async Task<DtoWorkingDays> GetByYear(int year)
        {
            return await DaoWorkingDaysFactory.GetDaoWorkingDay().GetByYear(year);
        }

        public static async Task<List<DtoWorkingDays>> GetAll()
        {
            return await DaoWorkingDaysFactory.GetDaoWorkingDay().GetAll();
        }
    }
}
