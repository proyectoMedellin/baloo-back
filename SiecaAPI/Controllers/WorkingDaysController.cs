using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO.Response;
using SiecaAPI.Models.Services;
using System.Drawing.Printing;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WorkingDaysController : ControllerBase
    {
        private readonly ILogger<WorkingDaysController> _logger;

        public WorkingDaysController(ILogger<WorkingDaysController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Configure")]
        public async Task<IActionResult> Configure(DtoWorkingDays request)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(await WorkingDaysServices.ConfigureAsync(request));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WorkingDaysController: configure -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int year)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(await WorkingDaysServices.DeleteByYearAsync(year));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WorkingDaysController: DeleteByYear -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetByYear")]
        public async Task<IActionResult> GetByYear(int year)
        {
            DtoRequestResult<DtoWorkingDays> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(await WorkingDaysServices.GetByYear(year));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WorkingDaysController: getByYear -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            DtoRequestResult<DtoWorkingDays> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await WorkingDaysServices.GetAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WorkingDaysController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
