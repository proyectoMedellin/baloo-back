namespace SiecaAPI.DTO.Response
{
    public class DtoLoginResp
    {
        public string Token { get; set; }

        public DtoLoginResp(string token)
        {
            Token = token;
        }
    }
}
