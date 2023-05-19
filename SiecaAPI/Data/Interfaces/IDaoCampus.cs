using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoCampus
    {
        public Task<DtoCampus> CreateAsync(DtoCampus campus);
        public Task<DtoCampus> UpdateAsync(DtoCampus campus);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoCampus>> GetAllAsync(int page, int pageSize, Guid? trainingCenterId, string? fCode,
            string? fName, bool? fEnabled);
        public Task<List<DtoCampus>> GetActiveByTrainingCenterAsync(Guid trainingCenterId);
        public Task<DtoCampus> GetByIdAsync(Guid id);
    }
}
