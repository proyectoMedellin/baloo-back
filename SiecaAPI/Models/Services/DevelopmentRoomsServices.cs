using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.Errors;
using SiecaAPI.Services;
using System;
using System.Collections.Generic;

namespace SiecaAPI.Models.Services
{
    public static class DevelopmentRoomsServices
    {
        public static async Task<DtoDevelopmentRoom> CreateAsync(DtoDevelopmentRoom room)
        {
            if(string.IsNullOrEmpty(room.Code) || string.IsNullOrEmpty(room.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoCampus cmp = await CampusesServices.GetByIdAsync(room.CampusId);
            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(cmp.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            room.OrganizationId = org.Id.Value;
            room.TrainingCenterId = tc.Id;
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().CreateAsync(room);
        }

        public static async Task<DtoDevelopmentRoom> UpdateAsync(DtoDevelopmentRoom room)
        {
            if (string.IsNullOrEmpty(room.Code) || string.IsNullOrEmpty(room.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoCampus cmp = await CampusesServices.GetByIdAsync(room.CampusId);
            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(cmp.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            room.OrganizationId = org.Id.Value;
            room.TrainingCenterId = tc.Id;
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().UpdateAsync(room);
        }

        public static async Task<List<DtoDevelopmentRoom>> GetAllAsync(int page, int pageSize, 
            Guid? campusId, string? fCode, string? fName, bool? fEnabled)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetAllAsync(page, pageSize,
                campusId, fCode, fName, fEnabled);
        }

        public static async Task<List<DtoDevelopmentRoom>> GetEnableDevRoomsByCampus(Guid campusId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetActiveByCampusAsync(campusId);
        }

        public static async Task<DtoDevelopmentRoom> GetByIdAsync(Guid campusId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetByIdAsync(campusId);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().DeleteByIdAsync(id);
        }

        public static async Task<List<DtoDevelopmentRoomGroupByYear>> GetGroupsByYear(Guid? DevRoomId, 
            int? year, int? page, int? pageSize)
        {
            if (year.HasValue && DateTime.TryParse(string.Format("1/1/{0}", year.Value), out _))
            {
                return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetAllGroupsByYear(DevRoomId, year, page, pageSize);
            }else if (!year.HasValue)
            {
                return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetAllGroupsByYear(DevRoomId, null, page, pageSize);
            }
            else
            {
                throw new MissingArgumentsException("Assignment cant be done with the current params");
            }
        }

        public static async Task<bool> AssignAgentsByYear(Guid DevRoomId, int year, string groupCode,
            string groupName, List<Guid> agentsIds, string assignmentUser)
        {
            if (DateTime.TryParse(string.Format("1/1/{0}", year), out _) 
                && !string.IsNullOrEmpty(groupCode) && !string.IsNullOrWhiteSpace(groupCode)
                && !string.IsNullOrEmpty(groupName) && !string.IsNullOrWhiteSpace(groupName)
                && agentsIds != null && agentsIds.Count > 0)
            {
                Organization org = await OrganizationServices.GetActiveOrganization();
                return await DaoDevelopmentRoomFactory.GetDaoDevelopment().AssignAgentsByYear(org.Id, DevRoomId, 
                    year, groupCode,groupName, agentsIds, assignmentUser);
            }
            else
            {
                throw new MissingArgumentsException("Assignment cant be done with the current params");
            }
        }

        public static async Task<bool> DeleteGroupAssignment(Guid groupAssignmentId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().DeleteGroupAssignment(groupAssignmentId);
        }

        public static async Task<bool> AssignBeneficiariesByYear(Guid developmentRoomGroupByYearId, 
            List<DtoBeneficiaryToAssign> beneficiariesList, string assignmentUser)
        {
            DtoDevelopmentRoomGroupByYear roomInfo = await DaoDevelopmentRoomFactory.GetDaoDevelopment()
                .GetGroupsYearAssignmentById(developmentRoomGroupByYearId);
            
            List<Guid> beneficiariesIds = new();
            foreach(DtoBeneficiaryToAssign ben in beneficiariesList)
            {
                beneficiariesIds.Add(ben.Id);
            }

            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().AssignBeneficiariesByYear(roomInfo.OrganizationId,
                roomInfo.TrainingCenterId, roomInfo.CampusId, roomInfo.DevelopmentRoomId, 
                developmentRoomGroupByYearId, beneficiariesIds, assignmentUser);
        }

        public static async Task<List<DtoBeneficiaries>> GetBeneficiariesByRoom(Guid developmentRoomGroupByYearId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetBeneficiariesByRoom(developmentRoomGroupByYearId);
        }

        public static async Task<DtoDevelopmentRoomGroupByYear> GetGroupsYearAssignmentById(Guid id)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetGroupsYearAssignmentById(id);
        }
    }
}
