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
    public class CampusController : ControllerBase
    {
        private readonly ILogger<CampusController> _logger;

        public CampusController(ILogger<CampusController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DtoCampusCreateReq request)
        {
            DtoRequestResult<DtoCampusCreateResp> response = new()
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
                DtoCampus newCampus = new()
                {
                    TrainingCenterId = request.TrainingCenterId,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    Enabled = request.Enabled,
                    CreatedBy = request.CreatedBy
                };
                newCampus = await CampusesServices.CreateAsync(newCampus);
                if (newCampus.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoCampusCreateResp() { 
                        Id = newCampus.Id,
                        OrganizationId = newCampus.OrganizationId,
                        TrainingCenterId=newCampus.TrainingCenterId,
                        Name = newCampus.Name,  
                        Code = newCampus.Code,
                        IntegrationCode = newCampus.IntegrationCode,
                        Enabled = newCampus.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The new campus was not created";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CampusController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DtoCampusUpdateReq request)
        {
            DtoRequestResult<DtoCampusCreateResp> response = new()
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
                DtoCampus campus = new()
                {
                    Id = request.Id,
                    TrainingCenterId = request.TrainingCenterId,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    Enabled = request.Enabled,
                    ModifiedBy = request.ModifiedBy,
                };
                campus = await CampusesServices.UpdateAsync(campus);
                if (campus.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoCampusCreateResp()
                    {
                        Id = campus.Id,
                        OrganizationId = campus.OrganizationId,
                        TrainingCenterId = campus.TrainingCenterId,
                        Name = campus.Name,
                        Code = campus.Code,
                        IntegrationCode = campus.IntegrationCode,
                        Enabled = campus.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The campus was not updated";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CampusController: Update -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DtoRequestResult<bool> response = new()
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
                response.Registros.Add(await CampusesServices.DeleteAsync(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CampusController: Delete -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid(int page, int pageSize, Guid? trainingCenterId, string? fCode,
            string? fName, bool? fEnabled)
        {
            DtoRequestResult<DtoCampusViewGridResp> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                string filterCode = !string.IsNullOrEmpty(fCode) && fCode != "null" ? fCode : string.Empty;
                string filterName = !string.IsNullOrEmpty(fName) && fName != "null" ? fName : string.Empty; 

                List<DtoCampus> tCenters = await CampusesServices
                    .GetAllAsync(page, pageSize, trainingCenterId, filterCode, filterName, fEnabled);

                foreach (DtoCampus dto in tCenters)
                {
                    response.Registros.Add(new DtoCampusViewGridResp() { 
                        Id = dto.Id,
                        TrainingCenterId = dto.TrainingCenterId,
                        TrainingCenterCode = dto.TrainingCenterCode,
                        TrainingCenterName = dto.TrainingCenterName,
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

        [HttpGet("GetEnableCampusesByTrainingCenter")]
        public async Task<IActionResult> GetEnableCampusesByTrainingCenter(Guid trainingCenterId)
        {
            DtoRequestResult<DtoCampus> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await CampusesServices.GetEnableCampusesByTrainingCenter(trainingCenterId);
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
            DtoRequestResult<DtoCampus> response = new DtoRequestResult<DtoCampus>
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

                response.Registros.Add(await CampusesServices.GetByIdAsync(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CampusController: getAllById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
