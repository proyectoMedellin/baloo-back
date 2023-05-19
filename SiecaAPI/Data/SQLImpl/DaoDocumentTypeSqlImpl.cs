using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoDocumentTypeSqlImpl : IDaoDocumentTypes
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

        public async Task<List<DTODocumentType>> GetAllAsync()
        {
            using SqlContext context = new SqlContext();
            List<DocumentTypeEntity> documentTypes = await context.DocumentTypes.Where(o => o.Enabled).ToListAsync();
            List<DTODocumentType> dtodocumentTypes = new();
            foreach (DocumentTypeEntity doc in documentTypes)
            {
                dtodocumentTypes.Add(new DTODocumentType(
                    doc.Id,
                    doc.OrganizationId,
                    doc.Code,
                    doc.Name,
                    doc.Enabled,
                    doc.CreatedBy,
                    doc.CreatedOn,
                    doc.ModifiedBy,
                    doc.ModifiedOn));
            }
            return dtodocumentTypes;
        }

        public Task<DTODocumentType> UpdateAsync(DTODocumentType documentType)
        {
            throw new NotImplementedException();
        }
    }
}
