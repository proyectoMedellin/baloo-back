using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO;
using SiecaAPI.Models.Services;
using System.Net;
using SiecaAPI.DssPro;
using System.Text.Json.Nodes;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DahuaDssProController : ControllerBase
    {
        private readonly ILogger<BeneficiariesController> _logger;

        public DahuaDssProController(ILogger<BeneficiariesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("UpdateFaceRecognitionDataByUser")]
        public async Task<IActionResult> UpdateFaceRecognitionDataByUser(string? documentNumber, DateTime? startDate, DateTime? endDate)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                await DssProServices.DownloadFaceRecognitionData(documentNumber, startDate, endDate);
                response.Registros.Add(true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
