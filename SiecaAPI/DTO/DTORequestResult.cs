namespace SiecaAPI.DTO
{
    public class DtoRequestResult<T>
    {
        public string CodigoRespuesta { get; set; } = string.Empty;
        public string MensajeRespuesta { get; set; } = string.Empty;
        public List<T> Registros { get; set; } = new List<T>();
    }
}