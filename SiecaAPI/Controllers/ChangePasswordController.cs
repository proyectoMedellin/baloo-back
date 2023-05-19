using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Response;
using SiecaAPI.Errors;
using SiecaAPI.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangePasswordController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public ChangePasswordController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetNewPassword")]
        public async Task<DtoRequestResult<DtoChangePasswordResp>> GetNewPasswordAsync(string userName, string password)
        {
            DtoRequestResult<DtoChangePasswordResp> response = new DtoRequestResult<DtoChangePasswordResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var userResp = await UsersServices.GetUserInfo(userName);
                    if (userResp.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");
                    bool passwordResp = await UsersServices.ChangePassword(userResp.Id.Value, password);
                    response.Registros.Add(new DtoChangePasswordResp(passwordResp));
                }
                else
                {
                    throw new MissingArgumentsException("el parametro userName no puede ser vacio");
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetNewPassword: GetNewPassword -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return response;
            }
        }
    }
}
