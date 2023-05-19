namespace SiecaAPI.DTO.Requests
{
    public class DtoLoginReq
    {
        public string UserData { get; set; } 

        public DtoLoginReq(string userData)
        {
            UserData = userData;
        }
    }
}
