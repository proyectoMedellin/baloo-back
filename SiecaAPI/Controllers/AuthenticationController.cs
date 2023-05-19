using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO.Response;
using SiecaAPI.Models.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public AuthenticationController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(DtoLoginReq request)
        {
            DtoRequestResult<DtoLoginResp> response = new DtoRequestResult<DtoLoginResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (string.IsNullOrEmpty(request.UserData) ||
                string.IsNullOrWhiteSpace(request.UserData))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "User data not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                string userName = await AuthenticationServices.IsValidLogin(request.UserData);
                if (!string.IsNullOrEmpty(userName)) {
                    string token = await AuthenticationServices.CreateUserToken(userName);
                    DtoLoginResp tResp = new(token);
                    response.Registros.Add(tResp);
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
    }
}
