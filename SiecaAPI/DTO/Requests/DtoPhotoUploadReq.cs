namespace SiecaAPI.DTO.Requests
{
    public class DtoPhotoUploadReq
    {
        public Guid BeneficiaryId { get; set; } = Guid.Empty;
        public string PhotoStream { get; set; } = string.Empty;
    }
}
