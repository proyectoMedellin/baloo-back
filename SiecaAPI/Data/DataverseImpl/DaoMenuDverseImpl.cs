using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoMenuDverseImpl : IDaoMenu
    {
        public Task<DtoMenu> CreateAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DtoMenu> GetItems(DtoMenu menu)
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoMenu>> GetItems()
        {
            throw new NotImplementedException();
        }

        public Task<List<DtoMenu>> GetItems(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<DtoMenu> UpdateAsync(DtoMenu menu)
        {
            throw new NotImplementedException();
        }
    }
}
