using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoDevelopmentRoom
    {
        public Task<DtoDevelopmentRoom> CreateAsync(DtoDevelopmentRoom devRoom);
        public Task<DtoDevelopmentRoom> UpdateAsync(DtoDevelopmentRoom devRoom);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoDevelopmentRoom>> GetAllAsync(int page, int pageSize, Guid? campusId, 
            string? fCode, string? fName, bool? fEnabled);
        public Task<List<DtoDevelopmentRoom>> GetActiveByCampusAsync(Guid campusId);
        public Task<DtoDevelopmentRoom> GetByIdAsync(Guid id);
        public Task<List<DtoDevelopmentRoomGroupByYear>> GetAllGroupsByYear(Guid? DevRoomId, int? year, int? page, int? pageSize);
        public Task<DtoDevelopmentRoomGroupByYear> GetGroupsYearAssignmentById(Guid id);
        public Task<bool> AssignAgentsByYear(Guid OrganizationId, Guid DevRoomId, int year, string groupCode,
            string groupName, List<Guid> agentsIds, string createdBy);
        public Task<bool> AssignBeneficiariesByYear(Guid organizationId, Guid trainingCenterId, Guid campusId,
            Guid developmentRoomId, Guid developmentRoomGroupByYearId, List<Guid> beneficiariesList, string assignmentUser);
        public Task<bool> DeleteGroupAssignment(Guid groupAssignmetId);
        public Task<List<DtoBeneficiaries>> GetBeneficiariesByRoom(Guid developmentRoomGroupByYearId);
    }
}
