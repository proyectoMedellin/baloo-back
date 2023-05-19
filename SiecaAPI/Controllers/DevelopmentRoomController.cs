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
    public class DevelopmentRoomController : ControllerBase
    {
        private readonly ILogger<DevelopmentRoomController> _logger;

        public DevelopmentRoomController(ILogger<DevelopmentRoomController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DtoDevRoomCreateReq request)
        {
            DtoRequestResult<DtoDevRoomCreateResp> response = new()
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
                DtoDevelopmentRoom newRoom = new()
                {
                    CampusId = request.CampusId,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    DahuaChannelCode = request.DahuaChannelCode,
                    Enabled = request.Enabled,
                    CreatedBy = request.CreatedBy
                };
                newRoom = await DevelopmentRoomsServices.CreateAsync(newRoom);
                if (newRoom.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoDevRoomCreateResp() { 
                        Id = newRoom.Id,
                        OrganizationId = newRoom.OrganizationId,
                        TrainingCenterId= newRoom.TrainingCenterId,
                        CampusId= newRoom.CampusId,
                        Name = newRoom.Name,  
                        Code = newRoom.Code,
                        IntegrationCode = newRoom.IntegrationCode,
                        DahuaChannelCode= newRoom.DahuaChannelCode,
                        Enabled = newRoom.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The new development room was not created";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DtoDevRoomUpdateReq request)
        {
            DtoRequestResult<DtoDevRoomCreateResp> response = new()
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
                DtoDevelopmentRoom room = new()
                {
                    Id = request.Id,
                    CampusId = request.CampusId,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    DahuaChannelCode = request.DahuaChannelCode,
                    Enabled = request.Enabled,
                    ModifiedBy = request.ModifiedBy,
                };
                room = await DevelopmentRoomsServices.UpdateAsync(room);
                if (room.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoDevRoomCreateResp()
                    {
                        Id = room.Id,
                        OrganizationId = room.OrganizationId,
                        TrainingCenterId = room.TrainingCenterId,
                        CampusId = room.CampusId,
                        Name = room.Name,
                        Code = room.Code,
                        IntegrationCode = room.IntegrationCode,
                        DahuaChannelCode = room.DahuaChannelCode,
                        Enabled = room.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The development room was not updated";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: Update -> " + ex.Message);
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
                response.Registros.Add(await DevelopmentRoomsServices.DeleteAsync(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: Delete -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid(int page, int pageSize, Guid? campusId, string? fCode,
            string? fName, bool? fEnabled)
        {
            DtoRequestResult<DtoDevRoomViewGridResp> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                string filterCode = !string.IsNullOrEmpty(fCode) && fCode != "null" ? fCode : string.Empty;
                string filterName = !string.IsNullOrEmpty(fName) && fName != "null" ? fName : string.Empty; 

                List<DtoDevelopmentRoom> rooms = await DevelopmentRoomsServices.GetAllAsync(page, pageSize, 
                    campusId, filterCode, filterName, fEnabled);

                foreach (DtoDevelopmentRoom room in rooms)
                {
                    response.Registros.Add(new DtoDevRoomViewGridResp() { 
                        Id = room.Id,
                        OrganizationId = room.OrganizationId,
                        TrainingCenterId = room.TrainingCenterId,
                        TrainingCenterCode = room.TrainingCenterCode,
                        TrainingCenterName = room.TrainingCenterName,
                        CampusId = room.CampusId,
                        CampusCode = room.CampusCode,
                        CampusName = room.CampusName,
                        Code = room.Code,    
                        Name = room.Name,
                        Enabled = room.Enabled
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: ViewGrid -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetEnableDevRoomsByCampus")]
        public async Task<IActionResult> GetEnableDevRoomsByCampus(Guid campusId)
        {
            DtoRequestResult<DtoDevelopmentRoom> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await DevelopmentRoomsServices.GetEnableDevRoomsByCampus(campusId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: GetEnableDevRoomsByCampus -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            DtoRequestResult<DtoDevelopmentRoom> response = new()
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

                response.Registros.Add(await DevelopmentRoomsServices.GetByIdAsync(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: getById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetGroupsByYear")]
        public async Task<IActionResult> GetGroupsByYear(Guid? DevRoomId, int? year, int? page, int? pageSize)
        {
            DtoRequestResult<DtoDevelopmentRoomGroupByYear> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await DevelopmentRoomsServices.GetGroupsByYear(DevRoomId, year, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: AssignAgentesByYearToDevRoom -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("AssignAgentesByYearToDevRoom")]
        public async Task<IActionResult> GroupAssignmentByYearToDevRoom(DtoDevRoomGroupYearCreateReq assignment)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(await DevelopmentRoomsServices.AssignAgentsByYear(
                    assignment.DevelopmentRoomId, assignment.Year, assignment.GroupCode, assignment.GroupName,
                    assignment.UsersIds, assignment.AssignmentUser));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: AssignAgentesByYearToDevRoom -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("AssignBeneficiariesByYearToDevRoom")]
        public async Task<IActionResult> AssignBeneficiariesByYearToDevRoom(DtoDevRoomGroupBeneficiariesCreateReq assignment)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(await DevelopmentRoomsServices.AssignBeneficiariesByYear(assignment.DevelopmentRoomGroupByYearId,
                    assignment.Beneficiaries, assignment.AssignamentUser));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: AssignBeneficiariesByYearToDevRoom -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetBeneficiariesByRoom")]
        public async Task<IActionResult> GetBeneficiariesByGroup(Guid groupAssignmentId)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await DevelopmentRoomsServices.GetBeneficiariesByRoom(groupAssignmentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: DeleteGroupAssignment -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("DeleteGroupAssignment")]
        public async Task<IActionResult> DeleteGroupAssignment(Guid groupAssignmentId)
        {
            DtoRequestResult<bool> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros.Add(
                    await DevelopmentRoomsServices.DeleteGroupAssignment(groupAssignmentId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: DeleteGroupAssignment -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetGroupsYearAssignmentById")]
        public async Task<IActionResult> GetGroupsYearAssignmentById(Guid id)
        {
            DtoRequestResult<DtoDevelopmentRoomGroupByYear> response = new()
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

                response.Registros.Add(await DevelopmentRoomsServices.GetGroupsYearAssignmentById(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DevelopmentRoomController: GetGroupsYearAssignmentById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
