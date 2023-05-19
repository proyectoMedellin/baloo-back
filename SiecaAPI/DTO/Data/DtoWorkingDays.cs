namespace SiecaAPI.DTO.Data
{
    public class DtoWorkingDays
    {
        public int Year { get; set; }
        public bool Monday { get; set; } = false;
        public bool Tuesday { get; set; } = false;
        public bool Wednesday { get; set; } = false;
        public bool Thursday { get; set; } = false;
        public bool Friday { get; set; } = false;
        public bool Saturday { get; set; } = false;
        public bool Sunday { get; set; } = false;
        public List<DtoHoliday> Holidays { get; set; } = new();
        public string ConfUser { get; set; } = string.Empty;
    }
}
