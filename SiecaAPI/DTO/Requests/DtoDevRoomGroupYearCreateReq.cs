namespace SiecaAPI.DTO.Requests
{
    public class DtoDevRoomGroupYearCreateReq
    {
        public Guid DevelopmentRoomId { set; get; }
        public int Year { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public List<Guid> UsersIds { get; set; } = new(); 
        public string AssignmentUser { get; set; } = string.Empty;
    }
}
