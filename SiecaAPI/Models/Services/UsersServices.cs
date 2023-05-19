using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class UsersServices
    {
        public static async Task<AccessUser> CreateAccessUserAsync(string userName, string email,
            string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requiredPaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, List<Guid> campusId, List<Guid> rolsId, bool globalUser)
        {
            Organization org = await OrganizationServices.GetActiveOrganization();

            DtoAccessUser user = new() {
                OrganizationId = org.Id,
                UserName = userName, 
                Email = email, 
                FirstName = firstName, 
                OtherNames = otherNames, 
                LastName = lastName,
                OtherLastName = otherLastName,
                RequirePasswordChange = requiredPaswordChange, 
                CreatedBy = createdBy, 
                Phone = phone, 
                DocumentTypeId = documentTypeId, 
                DocumentNo = documentNo, 
                TrainingCenterId = trainingCenterId, 
                CampusId = campusId, 
                RolsId = rolsId, 
                GlobalUser = globalUser
            };
            user = await DaoAccessUserFactory.GetDaoAccessUsers().CreateAsync(user);

            if (!user.Id.HasValue) throw new InvalidOperationException("El usuario no fue creada exitosamente");

            return new AccessUser(user.Id.Value, user.OrganizationId, user.UserName, user.Email, user.FirstName,
                user.OtherNames, user.LastName, user.OtherLastName, user.DocumentTypeId, user.DocumentNo, user.TrainingCenterId, user.GlobalUser);
        }

        public static async Task<DtoAccessUser> GetUserInfo(string userName)
        {
            DtoAccessUser user = await DaoAccessUserFactory
                .GetDaoAccessUsers().GetByUserNameAsync(userName);
            return user;
        }

        public static async Task<DtoAccessUser> GetById(Guid id)
        {
            DtoAccessUser user = await DaoAccessUserFactory
                .GetDaoAccessUsers().GetByIdAsync(id);
            return user;
        }

        public static async Task<bool> UpdateAccessUserAsync(Guid id, string userName, string email,
            string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requiredPaswordChange, string? phone, Guid documentTypeId, string documentNo, Guid? trainingCenterId, List<Guid>? campusId, 
            List<Guid> rolsId, bool globalUser)
        {

            DtoAccessUser user = new()
            {
                Id = id,
                UserName = userName,
                Email = email,
                FirstName = firstName,
                OtherNames = otherNames,
                LastName = lastName,
                OtherLastName = otherLastName,
                RequirePasswordChange = requiredPaswordChange,
                Phone = phone,
                DocumentTypeId = documentTypeId,
                DocumentNo = documentNo,
                TrainingCenterId = trainingCenterId,
                CampusId = campusId != null ? campusId : new List<Guid>(),
                RolsId = rolsId,
                GlobalUser = globalUser
            };
            
            bool response = await DaoAccessUserFactory.GetDaoAccessUsers().UpdateAsync(user);
            if (!response) throw new InvalidOperationException("El usuario no fue creada exitosamente");

            return response;

        }
        public static async Task<bool> ChangePassword(Guid id, string password)
        {
            bool canChangePassword = await DaoAccessUserFactory
                .GetDaoAccessUsers().UpdateUserPassword(id, password);
            return canChangePassword;
        }
        public static async Task<List<DtoAccessUser>> GetAllUsersByOrganization()
        {
            List<DtoAccessUser> users = await DaoAccessUserFactory.
                GetDaoAccessUsers().GetAllAsync();
            return users;
        }
        public static async Task<List<DtoAccessUser>> GetByTrainingCenterIdCapusId(Guid trainingCenterId, Guid campusId, string? roleName)
        {
            List<DtoAccessUser> users = await DaoAccessUserFactory.
                GetDaoAccessUsers().GetByTrainingCenterIdCampusIdAsync(trainingCenterId, campusId, roleName);
            return users;
        }
        public static async Task<bool> DeletedById(Guid id)
        {
            bool user = await DaoAccessUserFactory.
               GetDaoAccessUsers().DeleteByIdAsync(id);
            return user;
        }
        public static async Task<bool> ExistUserByDocument(Guid id, string document)
        {
            bool exist = await DaoAccessUserFactory.
                GetDaoAccessUsers().ExistUserByDocument(id, document);
            return exist;
        }
        public static async Task<bool> ExistUserByName(string UserName)
        {
            bool exist = await DaoAccessUserFactory.
                GetDaoAccessUsers().ExistUserByName(UserName);
            return exist;
        }
        public static async Task<bool> ExistUserByEmail(string email)
        {
            bool exist = await DaoAccessUserFactory.
                GetDaoAccessUsers().ExistsUserEmailAsync(email);
            return exist;
        }
    }
}
