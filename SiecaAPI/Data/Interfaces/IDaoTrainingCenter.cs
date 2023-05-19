using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoTrainingCenter
    {
        public Task<DtoTrainingCenter> CreateAsync(DtoTrainingCenter tcenter);
        public Task<DtoTrainingCenter> UpdateAsync(DtoTrainingCenter tcenter);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoTrainingCenter>> GetAllEnabledAsync();
        public Task<List<DtoTrainingCenter>> GetAllAsync(int page, int pageSize, string? fCode,
            string? fName, bool? fEnabled);
        public Task<DtoTrainingCenter> GetByIdAsync(Guid id);
    }
}
