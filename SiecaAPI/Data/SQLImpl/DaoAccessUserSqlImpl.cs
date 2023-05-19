using Microsoft.EntityFrameworkCore;
using SiecaAPI.Commons;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Models;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoAccessUserSqlImpl : IDaoAccessUsers
    {
        public async Task<DtoAccessUser> CreateAsync(DtoAccessUser user)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                if (user.OrganizationId == Guid.Empty) 
                    throw new MissingArgumentsException("Empty organization ID");

                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == user.OrganizationId).FirstAsync();                

                if (org != null)
                {
                    TrainingCenterEntity tra = new();
                    if (user.TrainingCenterId.HasValue && !user.TrainingCenterId.Equals(Guid.Empty))
                    {
                        Guid tId = user.TrainingCenterId.Value;
                        var tCenters = await context.TrainingCenters.Where(t => t.Id == tId).ToListAsync();
                        if (tCenters.Count > 0) tra = tCenters.First();
                    }

                    AccessUserEntity accessUser = new(org, user.UserName, user.Email, 
                        user.FirstName, user.OtherNames, user.LastName, user.OtherLastName, 
                        user.RequirePasswordChange, true, user.CreatedBy, DateTime.UtcNow, 
                        null, null, user.Phone, user.DocumentTypeId, user.DocumentNo, 
                        tra.Id.Equals(Guid.Empty) ? null : tra, user.GlobalUser);

                    await context.AccessUsers.AddAsync(accessUser);
                    await context.SaveChangesAsync();
    
                    user.Id = accessUser.Id;
                    string tmpPassword = SecurityTools.GeneratePassword();

                    AccessUserPasswordEntity password = new(accessUser.Id, tmpPassword, accessUser.CreatedBy, 
                        DateTime.UtcNow, null, null);
                    await context.AccessUsersPassword.AddAsync(password);
                    await context.SaveChangesAsync();

                    foreach( Guid rols in user.RolsId)
                    {
                        AccessUserRolEntity rolUser = new(accessUser.OrganizationId, null, accessUser.Id, rols);
                        await context.AccessUserRoles.AddAsync(rolUser);
                        await context.SaveChangesAsync();
                    }
                    foreach (Guid Campus in user.CampusId)
                    {
                        CampusByAccessUserEntity CampusUser = new(accessUser.OrganizationId, accessUser.TrainingCenterId, Campus, accessUser.Id);
                        await context.CampusByAccessUsers.AddAsync(CampusUser);
                        await context.SaveChangesAsync();
                    }
                    transaction.Commit();

                    return user;
                }
                throw new NoDataFoundException("No organization found");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

      

        public async Task<bool> DeleteAsync(DtoAccessUser org)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                if (id != Guid.Empty)
                {
                    AccessUserEntity? accessUser = await context.AccessUsers.FindAsync(id);
                    if (accessUser != null)
                    {
                        List<CampusByAccessUserEntity> existingCampuses = await context.CampusByAccessUsers
                        .Where(cu => cu.AccessUserId == accessUser.Id).ToListAsync();
                        if (existingCampuses.Count > 0)
                        {
                            context.CampusByAccessUsers
                                .FromSqlRaw(@"DELETE FROM \""CampusByAccessUser\"" WHERE \""AccessUserId\"" = '{accessUser.Id}'");
                            await context.SaveChangesAsync();
                        }

                        List<AccessUserRolEntity> existingRoles = await context.AccessUserRoles
                            .Where(ur => ur.AccessUserId == accessUser.Id).ToListAsync();
                        if (existingRoles.Count > 0)
                        {
                            context.AccessUserRoles.RemoveRange(existingRoles);
                            await context.SaveChangesAsync();
                        }

                        List<AccessUserPasswordEntity> passwords = await context.AccessUsersPassword
                            .Where(p => p.AccessUserId == accessUser.Id).ToListAsync();
                        if (passwords.Count > 0)
                        {
                            context.AccessUsersPassword.RemoveRange(passwords);
                            await context.SaveChangesAsync();
                        }
                        context.AccessUsers.FromSqlRaw(@"DELETE FROM \""AccessUsers\"" WHERE \""Id\"" = '{accessUser.Id}'");
                        await context.SaveChangesAsync();

                        transaction.Commit();   
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> ExistUserByDocument(Guid id, string document)
        {
            bool result = false;

            if (string.IsNullOrEmpty(document)) throw new MissingArgumentsException("Empty document");
            using SqlContext context = new SqlContext();

            List<AccessUserEntity> user = await context.AccessUsers
                .Where(u => u.DocumentTypeId == id)
                .Where(u => u.DocumentNo == document)
                .ToListAsync();
            if (user != null && user.Count > 0) result = true;

            return result;
        }

        public async Task<bool> ExistUserByName(string userName)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userName)) throw new MissingArgumentsException("Empty username");
            using SqlContext context = new SqlContext();

            List<AccessUserEntity> user = await context.AccessUsers
                .Where(u => u.UserName == userName)
                .ToListAsync();
            if (user != null && user.Count > 0) result = true;

            return result;
        }

        public async Task<bool> ExistsUserEmailAsync(string email)
        {
            bool result = false;

            if (string.IsNullOrEmpty(email)) throw new MissingArgumentsException("Empty email");
            using SqlContext context = new SqlContext();

            List<AccessUserEntity> user = await context.AccessUsers
                .Where(u => u.Email.Equals(email))
                .ToListAsync();
            if (user != null && user.Count > 0) result = true;

            return result;
        }

        public async Task<bool> ExistUserByUserNamePass(string userName, string pass)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userName))
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();

            if (user == null) throw new NoDataFoundException("No user found");

            AccessUserPasswordEntity passInfo = await context.AccessUsersPassword
                .Where(p => p.AccessUserId == user.Id).FirstAsync();

            if (passInfo == null) throw new NoDataFoundException("No pass found for user");

            result = passInfo.Password == pass;

            return result;
        }

        public async Task<List<DtoAccessUser>> GetAllAsync()
        {
            using SqlContext context = new SqlContext();
            OrganizationEntity org = await context.Organizations.FirstAsync();
            List<AccessUserEntity> users = await context.AccessUsers.Where(u => u.OrganizationId == org.Id).ToListAsync();
            List<DtoAccessUser> dtoAccessUser = new();
            foreach (AccessUserEntity user in users)
            {
                dtoAccessUser.Add(new DtoAccessUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    OtherNames = user.OtherNames,
                    LastName = user.LastName,
                    OtherLastName = user.OtherLastName,
                    RequirePasswordChange = user.RequirePaswordChange,
                    CreatedBy = user.CreatedBy,
                    Phone = user.Phone,
                    DocumentTypeId = user.DocumentTypeId,
                    DocumentNo = user.DocumentNo,
                    TrainingCenterId = user.TrainingCenterId,
                    GlobalUser = user.GlobalUser
                });
            }
            return dtoAccessUser;
        }

        public async Task<DtoAccessUser> GetByIdAsync(Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new MissingArgumentsException("Empty user id");

            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.Id.Equals(id)).FirstAsync();
            List<CampusByAccessUserEntity> campus = await context.CampusByAccessUsers.Where(camp => camp.AccessUserId == user.Id).ToListAsync();
            List<Guid> CampusId = new();
            foreach (var camp in campus)
            {
                CampusId.Add(camp.CampusId);
            }
            List<AccessUserRolEntity> rols = await context.AccessUserRoles.Where(rol => rol.AccessUserId == user.Id).ToListAsync();
            List<Guid> RolsId = new();
            foreach (var rol in rols)
            {
                RolsId.Add(rol.RolId);
            }
            return new DtoAccessUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                OtherNames = user.OtherNames,
                LastName = user.LastName,
                OtherLastName = user.OtherLastName,
                RequirePasswordChange = user.RequirePaswordChange,
                CreatedBy = user.CreatedBy,
                Phone = user.Phone,
                DocumentTypeId = user.DocumentTypeId,
                DocumentNo = user.DocumentNo,
                TrainingCenterId = user.TrainingCenterId,
                CampusId = CampusId,
                RolsId = RolsId,
                GlobalUser = user.GlobalUser
            };
        }

        public async Task<List<DtoAccessUser>> GetByTrainingCenterIdCampusIdAsync(Guid trainingCenterId, Guid campusId, string? roleName)
        {
            using SqlContext context = new SqlContext();
            List<AccessUserEntity> users = await context.CampusByAccessUsers
                .Where(x => x.TrainingCenterId == trainingCenterId && x.CampusId == campusId)
                .Join(context.AccessUserRoles,
                    campusUser => campusUser.AccessUserId,
                    accessUserRole => accessUserRole.AccessUserId,
                    (campusUser, accessUserRole) => new { CampusUser = campusUser, AccessUserRole = accessUserRole })
                .Where(x => roleName == null || x.AccessUserRole.Rol.Name.Contains(roleName))
                .Select(x => x.CampusUser.AccessUser).Distinct().ToListAsync();
            List<DtoAccessUser> dtoAccessUser = new();
            foreach (AccessUserEntity user in users)
            {
                dtoAccessUser.Add(new DtoAccessUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    OtherNames = user.OtherNames,
                    LastName = user.LastName,
                    OtherLastName = user.OtherLastName,
                    RequirePasswordChange = user.RequirePaswordChange,
                    CreatedBy = user.CreatedBy,
                    Phone = user.Phone,
                    DocumentTypeId = user.DocumentTypeId,
                    DocumentNo = user.DocumentNo,
                    TrainingCenterId = user.TrainingCenterId,
                    GlobalUser = user.GlobalUser
                });
            }
            return dtoAccessUser;
        }

        public async Task<DtoAccessUser> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new MissingArgumentsException("Empty user name");
            
            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();
            List<CampusByAccessUserEntity> campus = await context.CampusByAccessUsers.Where(camp => camp.AccessUserId == user.Id).ToListAsync();
            List<Guid> CampusId = new();
            foreach(var camp in campus)
            {
                CampusId.Add(camp.CampusId);
            }
            List<AccessUserRolEntity> rols = await context.AccessUserRoles.Where(rol=> rol.AccessUserId == user.Id).ToListAsync();
            List<Guid> RolsId= new();
            foreach (var rol in rols)
            {
                RolsId.Add(rol.RolId);
            }
            return new DtoAccessUser() {
                Id = user.Id, 
                UserName = user.UserName, 
                Email = user.Email, 
                FirstName = user.FirstName, 
                OtherNames = user.OtherNames,
                LastName = user.LastName, 
                OtherLastName = user.OtherLastName, 
                RequirePasswordChange = user.RequirePaswordChange, 
                CreatedBy = user.CreatedBy,
                Phone = user.Phone, 
                DocumentTypeId = user.DocumentTypeId, 
                DocumentNo = user.DocumentNo, 
                TrainingCenterId = user.TrainingCenterId, 
                CampusId = CampusId, 
                RolsId = RolsId, 
                GlobalUser = user.GlobalUser
            };
        }

        public async Task<List<DtoUserRol>> GetRolesByUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();

            //si el usuario existe, se considera un usuario interno, de lo contrario
            //se obtendrá la información de roles como un usuario externo
            List<AccessUserRolEntity> userRoles = user != null && user.Id != Guid.Empty ?
                    await context.AccessUserRoles.Where(aur => aur.AccessUserId == user.Id).ToListAsync() :
                    await context.AccessUserRoles.Where(aur => aur.AccessUserExternalId == userName).ToListAsync();

            List<DtoUserRol> roles = new List<DtoUserRol>();
            foreach (AccessUserRolEntity aur in userRoles)
            {
                RolEntity rol = await context.Roles.Where(r => r.Id.Equals(aur.RolId)).FirstAsync();
                roles.Add(new DtoUserRol()
                {
                    OrganizationId = aur.OrganizationId,
                    RolId = aur.RolId,
                    RolName = rol.Name
                });
            }
            return roles;
        }

        public async Task<bool> UpdateAsync(DtoAccessUser user)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                List<AccessUserEntity> accessUserList = await context.AccessUsers
                    .Where(au => au.Id.Equals(user.Id))
                    .Include(au => au.Organization)
                    .Include(au => au.TrainingCenter)
                    .ToListAsync();

                if (accessUserList != null && accessUserList.Count > 0)
                {
                    AccessUserEntity accessUser = accessUserList[0];

                    List<CampusByAccessUserEntity> existingCampuses = await context.CampusByAccessUsers
                        .Where(cu => cu.AccessUserId == accessUser.Id).ToListAsync();
                    if (existingCampuses.Count > 0)
                    {
                        context.CampusByAccessUsers
                            .FromSqlRaw(@"DELETE FROM \""CampusByAccessUser\"" WHERE \""AccessUserId\"" = '{accessUser.Id}'");
                        await context.SaveChangesAsync();
                    }

                    List<AccessUserRolEntity> existingRoles = await context.AccessUserRoles
                        .Where(ur => ur.AccessUserId == accessUser.Id).ToListAsync();
                    if (existingRoles.Count > 0)
                    {
                        context.AccessUserRoles.RemoveRange(existingRoles);
                        await context.SaveChangesAsync();
                    }

                    accessUser.UserName = user.UserName;
                    accessUser.Email = user.Email;
                    accessUser.FirstName = user.FirstName;
                    accessUser.OtherNames = user.OtherNames;
                    accessUser.LastName = user.LastName;
                    accessUser.OtherLastName = user.OtherLastName;
                    accessUser.ModifiedOn = DateTime.UtcNow;
                    accessUser.Phone = user.Phone;
                    accessUser.DocumentTypeId = user.DocumentTypeId;
                    accessUser.DocumentNo = user.DocumentNo;
                    if (user.TrainingCenterId != Guid.Empty)
                    {
                        TrainingCenterEntity tc = await context.TrainingCenters
                            .Where(tc => tc.Id.Equals(user.TrainingCenterId)).FirstOrDefaultAsync();
                        if(tc != null && !tc.Id.Equals(Guid.Empty))
                        {
                            accessUser.TrainingCenterId = user.TrainingCenterId;
                            accessUser.TrainingCenter = tc;
                        }
                        
                    }
                    await context.SaveChangesAsync();

                    foreach (Guid rols in user.RolsId)
                    {
                        AccessUserRolEntity rolUser = new(accessUser.OrganizationId, null, accessUser.Id, rols);
                        await context.AccessUserRoles.AddAsync(rolUser);
                        await context.SaveChangesAsync();
                    }
                    foreach (Guid Campus in user.CampusId)
                    {
                        string insertQuery = "INSERT INTO \"CampusByAccessUser\"(\"OrganizationId\",\"TrainingCenterId\",\"CampusId\",\"AccessUserId\") " +
                            "VALUES " +
                            @"('{accessUser.OrganizationId}','{accessUser.TrainingCenterId}','{campus}','{accessUser.Id}')";
                        context.CampusByAccessUsers.FromSqlRaw(insertQuery);
                        await context.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return true;
                }
                throw new NoDataFoundException("No user found");
            }
            catch
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserPassword(Guid? id, string password)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserPasswordEntity accessUser = await context.AccessUsersPassword.Where(u => u.AccessUserId == id).FirstAsync();
            accessUser.Password = password;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
