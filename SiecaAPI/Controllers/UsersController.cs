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
    public class UsersController : ControllerBase
    {
        
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }
        [HttpPost("CreateAccessUser")]
        public async Task<DtoRequestResult<DtoAccessUserResp>> CreateAccessUser(DtoAccessUserReq request)
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.CreatedBy)
                    && !string.IsNullOrEmpty(request.FirstName) && !string.IsNullOrEmpty(request.LastName))
                {
                    AccessUser user = await UsersServices.CreateAccessUserAsync(request.UserName, request.Email, 
                        request.FirstName, request.OtherNames,
                        request.LastName, request.OtherLastName, true, request.CreatedBy, request.Phone, 
                        request.DocumentTypeId, request.DocumentNo, 
                        request.TrainingCenterId, request.CampusId, request.RolsId, request.GlobalUser);
                    response.Registros.Add(
                        new DtoAccessUserResp()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            OtherNames = user.OtherNames,
                            LastName = user.LastName,
                            OtherLastName = user.OtherLastName,
                            DocumentTypeId = user.DocumentTypeId,
                            DocumentNo = user.DocumentNo,
                            TrainingCenterId = user.TrainingCenterId,
                            GlobalUser = user.GlobalUser
                        });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateAccessUser: CreateAccessUser -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return response;
            }
        }

        [HttpGet("GetAccessUserByName")]
        public async Task<IActionResult> GetAccessUserByName(string userName)
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var user = await UsersServices.GetUserInfo(userName);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");

                    response.Registros.Add(
                        new DtoAccessUserResp()
                        {
                            Id = user.Id.Value,
                            OrganizationId = user.OrganizationId,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            OtherNames = user.OtherNames,
                            LastName = user.LastName,
                            OtherLastName = user.OtherLastName,
                            DocumentTypeId = user.DocumentTypeId,
                            DocumentNo = user.DocumentNo,
                            TrainingCenterId = user.TrainingCenterId,
                            CampusId = user.CampusId,
                            RolsId = user.RolsId,
                            Phone = user.Phone,
                            GlobalUser = user.GlobalUser
                        });
                }
                else
                {
                    throw new MissingArgumentsException("el parametro userName no puede ser vacio");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController: GetAccessUserByName -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("UpdateAccessUser")]
        public async Task<IActionResult> UpdateAccessUser(DtoAccessUserReq request)  
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!request.Id.Equals(Guid.Empty) && !string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Email) 
                    && !string.IsNullOrEmpty(request.FirstName) && !string.IsNullOrEmpty(request.LastName))
                {
                    var user = await UsersServices.GetById(request.Id);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");
                    
                    await UsersServices.UpdateAccessUserAsync(request.Id,request.UserName, request.Email, request.FirstName, request.OtherNames,
                        request.LastName, request.OtherLastName, true, request.Phone, request.DocumentTypeId, request.DocumentNo,
                        request.TrainingCenterId, request.CampusId, request.RolsId, request.GlobalUser);

                    response.Registros.Add(
                        new DtoAccessUserResp()
                        {
                            Id = user.Id.Value,
                            OrganizationId = user.OrganizationId,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            OtherNames = user.OtherNames,
                            LastName = user.LastName,
                            OtherLastName = user.OtherLastName,
                            DocumentTypeId = user.DocumentTypeId,
                            DocumentNo = user.DocumentNo,
                            TrainingCenterId = user.TrainingCenterId,
                            GlobalUser = user.GlobalUser
                        });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthenticationController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            DtoRequestResult<List<DtoAccessUserResp>> response = new DtoRequestResult<List<DtoAccessUserResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoAccessUser> accessUsers = await UsersServices.GetAllUsersByOrganization();
                List<DtoAccessUserResp> dtoAccessUserResp = new();
                if(accessUsers.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoAccessUser user in accessUsers)
                {
                    dtoAccessUserResp.Add(
                        new DtoAccessUserResp()
                        {
                            Id = user.Id.Value,
                            OrganizationId = user.OrganizationId,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            OtherNames = user.OtherNames,
                            LastName = user.LastName,
                            OtherLastName = user.OtherLastName,
                            DocumentTypeId = user.DocumentTypeId,
                            DocumentNo = user.DocumentNo,
                            TrainingCenterId = user.TrainingCenterId,
                            GlobalUser = user.GlobalUser
                        });
                }
                response.Registros.Add(new List<DtoAccessUserResp>(dtoAccessUserResp));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthenticationController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }
        [HttpGet("GetByTrainingCenterIdCapusId")]
        public async Task<IActionResult> GeByTrainingCenterIdCapusId(Guid trainingCenterId, Guid campusId, string? roleName)
        {
            DtoRequestResult<List<DtoAccessUserResp>> response = new DtoRequestResult<List<DtoAccessUserResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoAccessUser> accessUsers = await UsersServices.GetByTrainingCenterIdCapusId(trainingCenterId, campusId, roleName);
                List<DtoAccessUserResp> dtoAccessUserResp = new();
                //if (accessUsers.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoAccessUser user in accessUsers)
                {
                    dtoAccessUserResp.Add(
                        new DtoAccessUserResp()
                        {
                            Id = user.Id.Value,
                            OrganizationId = user.OrganizationId,
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            OtherNames = user.OtherNames,
                            LastName = user.LastName,
                            OtherLastName = user.OtherLastName,
                            DocumentTypeId = user.DocumentTypeId,
                            DocumentNo = user.DocumentNo,
                            TrainingCenterId = user.TrainingCenterId,
                            GlobalUser = user.GlobalUser
                        });
                }
                response.Registros.Add(new List<DtoAccessUserResp>(dtoAccessUserResp));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthenticationController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }
    
        [HttpGet("DeletedUser")]
        public async Task<IActionResult> DeletedUser(string userName)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var user = await UsersServices.GetUserInfo(userName);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");
                    bool deleted = await UsersServices.DeletedById(user.Id.Value);
                    response.Registros.Add(deleted);
                }
                else
                {
                    throw new MissingArgumentsException("el parametro userName no puede ser vacio");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Deleted: Deleted -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
        [HttpGet("ExistUserByDocument")]
        public async Task<IActionResult> ExistUserByDocument(Guid id, string document)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(document))
                {
                    bool exist = await UsersServices.ExistUserByDocument(id, document);
                    response.Registros.Add(exist);
                }
                else
                {
                    throw new MissingArgumentsException("el parametro document no puede ser vacio");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController: ExistUserByDocument -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
        [HttpGet("ExistUserByName")]
        public async Task<IActionResult> ExistUserByName(string userName)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    bool exist = await UsersServices.ExistUserByName(userName);
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
                _logger.LogError("UserController: ExistUserByDocument -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ExistUserByEmail")]
        public async Task<IActionResult> ExistUserByEmail(string email)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool exist = await UsersServices.ExistUserByEmail(email);
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
                _logger.LogError("UserController: ExistUserByDocument -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}