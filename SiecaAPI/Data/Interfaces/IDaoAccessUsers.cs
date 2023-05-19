using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoAccessUsers
    {
        public Task<DtoAccessUser> CreateAsync(DtoAccessUser user);
        public Task<bool> UpdateAsync(DtoAccessUser user);
        public Task<bool> DeleteAsync(DtoAccessUser user);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<DtoAccessUser> GetByIdAsync(Guid id);
        public Task<DtoAccessUser> GetByUserNameAsync(string userName);
        public Task<List<DtoAccessUser>> GetAllAsync();
        public Task<bool> UpdateUserPassword(Guid? id, string password);
        public Task<bool> ExistUserByUserNamePass(string userName, string pass);
        public Task<List<DtoUserRol>> GetRolesByUser(string userName);
        public Task<bool> ExistUserByDocument(Guid id, string document);
        public Task<bool> ExistsUserEmailAsync(string email);
        public Task<bool> ExistUserByName(string UserName);
        public Task<List<DtoAccessUser>> GetByTrainingCenterIdCampusIdAsync(Guid trainingCenterId, Guid campusId, string? roleName);
    }
}
