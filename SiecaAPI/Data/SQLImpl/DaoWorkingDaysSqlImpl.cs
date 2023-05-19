using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoWorkingDaysSqlImpl : IDaoWorkingDays
    {
        public async Task<bool> ConfigureAsync(DtoWorkingDays wd)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                //elimino las configuraciones previas para el año
                context.Holidays.RemoveRange(
                        await context.Holidays.Where(h => h.Year.Equals(wd.Year)).ToListAsync()
                    );
                await context.SaveChangesAsync();

                context.WorkingDaysOfWeek.RemoveRange(
                        await context.WorkingDaysOfWeek.Where(h => h.Year.Equals(wd.Year)).ToListAsync()
                    );
                await context.SaveChangesAsync();

                //se crea la nueva configuracion
                WorkingDayOfWeekEntity newWdw = new()
                {
                    Year = wd.Year,
                    Monday = wd.Monday,
                    Tuesday = wd.Tuesday,
                    Wednesday = wd.Wednesday,
                    Thursday = wd.Thursday,
                    Friday = wd.Friday,
                    Saturday = wd.Saturday,
                    Sunday = wd.Sunday,
                    CreatedBy = wd.ConfUser,
                    CreatedOn = DateTime.UtcNow
                };
                await context.WorkingDaysOfWeek.AddAsync(newWdw);
                await context.SaveChangesAsync();

                List<HolidayEntity> hToCreate = new();
                foreach (DtoHoliday h in wd.Holidays) {
                    hToCreate.Add(new HolidayEntity()
                    {
                        Year = newWdw.Year,
                        Day = h.Day.ToUniversalTime(),
                        CreatedBy = wd.ConfUser,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                await context.Holidays.AddRangeAsync(hToCreate);
                await context.SaveChangesAsync();

                transaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteByYearAsync(int year)
        {
            using SqlContext context = new();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                //elimino las configuraciones previas para el año
                context.Holidays.RemoveRange(
                        await context.Holidays.Where(h => h.Year.Equals(year)).ToListAsync()
                    );
                await context.SaveChangesAsync();

                context.WorkingDaysOfWeek.RemoveRange(
                        await context.WorkingDaysOfWeek.Where(h => h.Year.Equals(year)).ToListAsync()
                    );
                await context.SaveChangesAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<DtoWorkingDays> GetByYear(int year)
        {
            DtoWorkingDays response = new();

            using SqlContext context = new();

            List<WorkingDayOfWeekEntity> workdays = await context.WorkingDaysOfWeek.Where(wdw => wdw.Year.Equals(year)).ToListAsync();
            if (workdays != null && workdays.Count > 0)
            {
                WorkingDayOfWeekEntity wdw = workdays.First();
                List<HolidayEntity> holidays = await context.Holidays.Where(h => h.Year.Equals(year)).ToListAsync();

                response = new()
                {
                    Year = wdw.Year,
                    Monday = wdw.Monday,
                    Tuesday = wdw.Tuesday,
                    Wednesday = wdw.Wednesday,
                    Thursday = wdw.Thursday,
                    Friday = wdw.Friday,
                    Saturday = wdw.Saturday,
                    Sunday = wdw.Sunday,
                };

                foreach (HolidayEntity h in holidays)
                {
                    response.Holidays.Add(new DtoHoliday() { Day = h.Day});
                }
                
            }

            return response;
        }

        public async Task<List<DtoWorkingDays>> GetAll()
        {

            List<DtoWorkingDays> response = new();

            using SqlContext context = new();
            List<WorkingDayOfWeekEntity> wdwList = await context.WorkingDaysOfWeek.ToListAsync();
            foreach (WorkingDayOfWeekEntity wdw in wdwList)
            {
                List<HolidayEntity> holidays = await context.Holidays.Where(h => h.Year.Equals(wdw.Year)).ToListAsync();
                DtoWorkingDays yearInfo = new()
                {
                    Year = wdw.Year,
                    Monday = wdw.Monday,
                    Tuesday = wdw.Tuesday,
                    Wednesday = wdw.Wednesday,
                    Thursday = wdw.Thursday,
                    Friday = wdw.Friday,
                    Saturday = wdw.Saturday,
                    Sunday = wdw.Sunday,
                };

                foreach (HolidayEntity h in holidays)
                {
                    yearInfo.Holidays.Add(new DtoHoliday() { Day = h.Day });
                }
                
                response.Add(yearInfo);
            }

            return response;
        }
    }
}
