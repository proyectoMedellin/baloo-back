using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoWorkingDays
    {
        public Task<bool> ConfigureAsync(DtoWorkingDays wd);
        public Task<bool> DeleteByYearAsync(int year);
        Task<DtoWorkingDays> GetByYear(int year);
        public Task<List<DtoWorkingDays>> GetAll();
    }
}
