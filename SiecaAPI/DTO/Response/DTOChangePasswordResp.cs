namespace SiecaAPI.DTO.Response
{
    public class DtoChangePasswordResp
    {
        public bool CanChange { get; set; }

        public DtoChangePasswordResp(bool canChange)
        {
            CanChange = canChange;
        }
    }
}
