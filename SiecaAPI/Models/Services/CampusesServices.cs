using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Response;
using SiecaAPI.Errors;
using SiecaAPI.Services;
using System.ComponentModel;

namespace SiecaAPI.Models.Services
{
    public static class CampusesServices
    {
        public static async Task<DtoCampus> CreateAsync(DtoCampus campus)
        {
            if(string.IsNullOrEmpty(campus.Code) || string.IsNullOrEmpty(campus.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(campus.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            campus.OrganizationId = org.Id.Value;
            campus.TrainingCenterId = tc.Id;

            return await DaoCampusFactory.GetDaoCampus().CreateAsync(campus);
        }

        public static async Task<DtoCampus> UpdateAsync(DtoCampus campus)
        {
            if (string.IsNullOrEmpty(campus.Code) || string.IsNullOrEmpty(campus.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(campus.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            campus.OrganizationId = org.Id.Value;
            campus.TrainingCenterId = tc.Id;

            return await DaoCampusFactory.GetDaoCampus().UpdateAsync(campus);
        }

        public static async Task<List<DtoCampus>> GetAllAsync(int page, int pageSize, 
            Guid? trainingCenterId, string? fCode, string? fName, bool? fEnabled)
        {
            return await DaoCampusFactory.GetDaoCampus().GetAllAsync(page, pageSize, 
                trainingCenterId, fCode, fName, fEnabled);
        }

        public static async Task<List<DtoCampus>> GetEnableCampusesByTrainingCenter(Guid trainingCenterId)
        {
            return await DaoCampusFactory.GetDaoCampus().GetActiveByTrainingCenterAsync(trainingCenterId);
        }

        public static async Task<DtoCampus> GetByIdAsync(Guid id)
        {
            return await DaoCampusFactory.GetDaoCampus().GetByIdAsync(id);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoCampusFactory.GetDaoCampus().DeleteByIdAsync(id);
        }
    }
}
