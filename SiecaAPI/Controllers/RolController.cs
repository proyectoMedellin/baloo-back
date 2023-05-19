using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO.Response;
using SiecaAPI.Errors;
using SiecaAPI.Models;
using SiecaAPI.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly ILogger<RolController> _logger;

        public RolController(ILogger<RolController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllRol")]
        public async Task<IActionResult> GetAllRol()
        {
            DtoRequestResult<List<DtoRolResp>> response = new DtoRequestResult<List<DtoRolResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoRol> roles = await RolServices.GetAllRolAsync();
                List<DtoRolResp> dtoRolesResp = new();
                if (roles.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoRol rol in roles)
                {
                    dtoRolesResp.Add(new DtoRolResp(
                        rol.Id,
                        rol.OrganizationId,
                        rol.Name,
                        rol.Description,
                        rol.NewAccessUserDefaultRol,
                        rol.Enabled,
                        rol.CreatedBy,
                        rol.CreatedOn,
                        rol.ModifiedBy,
                        rol.ModifiedOn));
                }
                response.Registros.Add(new List<DtoRolResp>(dtoRolesResp));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("RolesController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }

        [HttpPost("CreateRol")]
        public async Task<IActionResult> CreateRol(DtoRolCreateReq request)
        {
            DtoRequestResult<DtoRolResp> response = new DtoRequestResult<DtoRolResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrEmpty(request.CreatedBy))
                {
                    
                    Rol rol = await RolServices.CreateRolAsync(request.Name, request.Description, 
                        request.NewAccessUserDefaultRol, request.CreatedBy, request.ListPermission);

                    response.Registros.Add(new DtoRolResp(rol.Id, rol.OrganizationId, rol.Name, rol.Description, 
                        rol.NewAccessUserDefaultRol, rol.Enable, rol.CreatedBy, rol.CreatedOn, rol.ModifiedBy, rol.ModifiedOn));
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateRol: Createrol -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }

        [HttpPost("UpdateRol")]
        public async Task<IActionResult> UpdateRol(DtoRolUpdateReq request)
        {
            DtoRequestResult<DtoRolResp> response = new DtoRequestResult<DtoRolResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrEmpty(request.ModifiedBy))
                {
                    DtoRol rol = await RolServices.UpdateRolAsync(request.RolId,request.Name, request.Description, 
                        request.NewAccessUserDefaultRol, request.Enabled, request.ModifiedBy, request.ListPermission);

                    response.Registros.Add(new DtoRolResp(rol.IsUpdate));
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateRol: UpdateRol -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }
        [HttpGet("IsUserAdmin")]
        public async Task<IActionResult> IsAdmin(string userName)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    bool exist = await RolServices.GetUserIsAdmin(userName);
                    response.Registros.Add(exist);
                }
                else
                {
                    throw new MissingArgumentsException("el parametro userName no puede ser vacio");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("RolController: IsAdmin -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

    }
}
