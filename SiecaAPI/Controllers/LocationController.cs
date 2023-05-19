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
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetCountryList")]
        public async Task<IActionResult> GetCountryList()
        {
            DtoRequestResult<DtoCountry> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await LocationServices.GetCountries(); 
               return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("LocationController: GetCountryList -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetDeparmentsByCountry")]
        public async Task<IActionResult> GetDeparmentsByCountry(Guid countryId)
        {
            DtoRequestResult<DtoDepartment> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await LocationServices.GetDeparmentsByCountry(countryId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("LocationController: GetDeparmentsByCountry -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetCitiesByDeparment")]
        public async Task<IActionResult> GetCitiesByDeparment(Guid departmentId)
        {
            DtoRequestResult<DtoCity> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await LocationServices.GetCitiesByDeparment(departmentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("LocationController: GetCitiesByDeparment -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
