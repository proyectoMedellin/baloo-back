using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoDocumentTypeDverseImpl : IDaoDocumentTypes
    {
        public Task<DTODocumentType> CreateAsync(DTODocumentType documentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DTODocumentType documentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DTODocumentType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DTODocumentType> UpdateAsync(DTODocumentType documentType)
        {
            throw new NotImplementedException();
        }
    }
}
