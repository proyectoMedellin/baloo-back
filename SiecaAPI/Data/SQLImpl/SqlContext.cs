using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SiecaAPI.Commons;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DssPro.DTO;

namespace SiecaAPI.Data.SQLImpl
{
    public class SqlContext : DbContext
    {
        public SqlContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //version sql server
            /*string conn = AppParamsTools.GetEnvironmentVariable("ConnectionStrings:sqlDB");
            optionsBuilder.UseSqlServer(conn);
            base.OnConfiguring(optionsBuilder);*/

            //version postgres
            string conn = AppParamsTools.GetEnvironmentVariable("ConnectionStrings:PgSqlDB");
            optionsBuilder.UseNpgsql(conn);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<AccessUserEntity> AccessUsers { get; set; }
        public DbSet<AccessUserPasswordEntity> AccessUsersPassword { get; set; }
        public DbSet<BeneficiaryAnthropometricDataEntity> BeneficiaryAnthropometricData { get; set; }
        public DbSet<DocumentTypeEntity> DocumentTypes { get; set; }
        public DbSet<RolEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolPermissionEntity> RolePermissions { get; set; }
        public DbSet<AccessUserRolEntity> AccessUserRoles { get; set; }
        public DbSet<MenuEntity> Menu { get; set; }
        public DbSet<TrainingCenterEntity> TrainingCenters { get; set; }
        public DbSet<CampusEntity> Campuses { get; set; }
        public DbSet<DevelopmentRoomEntity> DevelopmentRooms { get; set; }
        public DbSet<DevelopmentRoomGroupByYearEntity> DevelopmentRoomGroupByYears { get; set; }
        public DbSet<DevelopmentRoomGroupAgentEntity> DevelopmentRoomGroupAgents { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<BeneficiariesParametersEntity> BeneficiariesParameters { get; set; }
        public DbSet<BeneficiariesEntity> Beneficiaries { get; set; }
        public DbSet<BeneficiariesFamilyEntity> BeneficiariesFamilies { get; set; }
        public DbSet<BeneficiaryAnthropometricDataEntity> BeneficiaryAnthropometricRecords { get; set; }
        public DbSet<BeneficiariesEmotionsRecordEntity> BeneficiariesEmotionsRecords { get; set; }
        public DbSet<CampusByAccessUserEntity> CampusByAccessUsers { get; set; }
        public DbSet<DevelopmentRoomGroupBeneficiaryEntity> DevelopmentRoomGroupBeneficiaries { get; set; }
        public DbSet<WorkingDayOfWeekEntity> WorkingDaysOfWeek { get; set; }
        public DbSet<HolidayEntity> Holidays { get; set; }
        public DbSet<EmotionRawDataEntity> EmotionRawDataEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolPermissionEntity>()
                .HasKey(rp => new { rp.OrganizationId, rp.RolId, rp.PermissionId });

            modelBuilder.Entity<CampusByAccessUserEntity>()
                .HasKey(ca => new { ca.OrganizationId, ca.CampusId, ca.TrainingCenterId });

            modelBuilder.Entity<DevelopmentRoomGroupAgentEntity>()
                .HasKey(drga => new { drga.DevelopmentRoomGroupByYearId, drga.AccessUserId });

            modelBuilder.Entity<DevelopmentRoomGroupBeneficiaryEntity>()
                .HasKey(drge => new { drge.DevelopmentRoomGroupByYearId, drge.BeneficiaryId });
        }
    }
}
