using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Services;
using System.Drawing.Printing;

namespace SiecaAPI.Models.Services
{
    public static class BeneficiariesServices
    {
        public static async Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary)
        {

            if(await DaoBeneficiariesFactory.GetDaoBeneficiaries()
                .ExistAsync(beneficiary.DocumentTypeId, beneficiary.DocumentNumber))
            {
                throw new ExistingBeneficiaryException("The beneficiary to create already exists");
            }


            Organization org = await OrganizationServices.GetActiveOrganization();
            beneficiary.OrganizationId = org.Id;
            
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().CreateAsync(beneficiary);
        }

        public static async Task<DtoBeneficiaries> UpdateAsync(DtoBeneficiaries beneficiary)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().UpdateAsync(beneficiary);
        }

        public static async Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetBeneficiaryParameterInfoByType(type);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().DeleteAsync(id);
        }

        public static async Task<DtoBeneficiaries> GetById(Guid id)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetById(id);
        }

        public static async Task<List<DtoBeneficiaries>> GetAllAsync(int? year, Guid? TrainingCenterId, Guid? CampusId,
            Guid? DevelopmentRoomId, Guid? documentType, string? documentNumber, string? name, string? group,
            bool? fEnabled, int? page, int? pageSize)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetAllAsync(year, TrainingCenterId, CampusId,
            DevelopmentRoomId, documentType, documentNumber, name, group, fEnabled, page, pageSize);
        }

        public static async Task<DtoBeneficiaries> UploadPhotoAsync(Guid beneficiaryId, string photoData)
        {
            DtoBeneficiaries resp = await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetById(beneficiaryId);

            if (resp == null || resp.Id.Equals(Guid.Empty))
            {
                throw new NoDataFoundException("the beneficiary is not valid");
            }
            
            return await UpdateBeneficiaryPhoto(beneficiaryId, photoData);
        }

        private static async Task<DtoBeneficiaries> UpdateBeneficiaryPhoto(Guid beneficiaryId, string url)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().UpdatePhotoUrl(beneficiaryId, url);
        }

        public static async Task<List<DtoBeneficiariesAnthropometricRecord>> GetAnthropometricDataFromBeneficiaryId(Guid beneficiaryId, DateTime from, DateTime to)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetAnthropometricDataFromBeneficiaryId(beneficiaryId, from, to);
        }

        public static async Task<List<DtoBeneficiariesEmotionsRecord>> GetEmotionsDataById(Guid beneficiaryId, DateTime from, DateTime to)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetEmotionsDataById(beneficiaryId, from, to);
        }

        public static async Task<List<DtoBeneficiariesEmotionsRecord>> GetAssistenceDataById(Guid beneficiaryId, DateTime from, DateTime to)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetAssistenceDataById(beneficiaryId, from, to);
        }
    }
}
