using Azure.Core;
using Microsoft.AspNetCore.Authentication;
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
using System.Security.Claims;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TrainingCenterController : ControllerBase
    {
        private readonly ILogger<TrainingCenterController> _logger;

        public TrainingCenterController(ILogger<TrainingCenterController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DtoTrainingCenterCreateReq request)
        {
            DtoRequestResult<DtoTrainingCenterCreateResp> response = new DtoRequestResult<DtoTrainingCenterCreateResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.Name))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Params not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }
                DtoTrainingCenter newCenter = new()
                {
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    Enabled = request.Enabled,
                    CreatedBy = request.CreatedBy
                };
                newCenter = await TrainingCenterServices.CreateAsync(newCenter);
                if (newCenter.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoTrainingCenterCreateResp() { 
                        Id = newCenter.Id,
                        Name = newCenter.Name,  
                        Code = newCenter.Code,
                        IntegrationCode = newCenter.IntegrationCode,
                        Enabled = newCenter.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The new center was not created";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DtoTrainingCenterUpdateReq request)
        {
            DtoRequestResult<DtoTrainingCenterCreateResp> response = new DtoRequestResult<DtoTrainingCenterCreateResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Code) || 
                    string.IsNullOrEmpty(request.Name))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Params not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }
                DtoTrainingCenter center = new()
                {
                    Id = request.Id,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    Enabled = request.Enabled,
                    ModifiedBy = request.ModifiedBy,
                };
                center = await TrainingCenterServices.UpdateAsync(center);
                if (center.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoTrainingCenterCreateResp()
                    {
                        Id = center.Id,
                        OrganizationId = center.OrganizationId,
                        Name = center.Name,
                        Code = center.Code,
                        IntegrationCode = center.IntegrationCode,
                        Enabled = center.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The new center was not updated";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: Update -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (id == Guid.Empty)
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Id not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }
                response.Registros.Add(await TrainingCenterServices.DeleteAsync(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: Delete -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid(int page, int pageSize, string? fCode,
            string? fName, bool? fEnabled)
        {
            var uName = ControllerTools.GetRequestUserName(HttpContext);

            DtoRequestResult<DtoTrainingCenterViewGridResp> response = new DtoRequestResult<DtoTrainingCenterViewGridResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                string filterCode = !string.IsNullOrEmpty(fCode) && fCode != "null" ? fCode : string.Empty;
                string filterName = !string.IsNullOrEmpty(fName) && fName != "null" ? fName : string.Empty; 

                List<DtoTrainingCenter> tCenters = await TrainingCenterServices
                    .GetAllAsync(page, pageSize, filterCode, filterName, fEnabled);

                foreach (DtoTrainingCenter dto in tCenters)
                {
                    response.Registros.Add(new DtoTrainingCenterViewGridResp() { 
                        Id = dto.Id,
                        Code = dto.Code,    
                        Name = dto.Name,
                        Enabled = dto.Enabled
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetEnabledTrainigCenterList")]
        public async Task<IActionResult> GetEnabledTrainigCenterList()
        {
            DtoRequestResult<DtoTrainingCenter> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await TrainingCenterServices.GetEnabledTrainigCenterList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            DtoRequestResult<DtoTrainingCenter> response = new DtoRequestResult<DtoTrainingCenter>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (id == Guid.Empty)
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Id not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                response.Registros.Add(await TrainingCenterServices.GetByIdAsync(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("TrainingCenterController: getAllById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
