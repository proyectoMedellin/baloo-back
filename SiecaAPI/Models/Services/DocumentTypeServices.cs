using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class DocumentTypeServices
    {

        public static async Task<List<DTODocumentType>> GetAllDocumentTypeAsync()
        {
            
            List<DTODocumentType> documentTypes = await DaoDocumentTypeFactory
                .GetDaoDocumenTypes().GetAllAsync();
            return documentTypes;
        }
    }
}
