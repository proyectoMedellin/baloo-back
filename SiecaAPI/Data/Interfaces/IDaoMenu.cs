using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoMenu
    {
        public Task<DtoMenu> CreateAsync(DtoMenu menu);
        public Task<DtoMenu> UpdateAsync(DtoMenu menu);
        public Task<List<DtoMenu>> GetItems(string userName);
        public Task<bool> DeleteAsync(DtoMenu menu);
        public Task<bool> DeleteByIdAsync(Guid id);
    }
}
