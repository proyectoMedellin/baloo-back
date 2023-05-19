using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
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

    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(DtoNotification request)
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var user = await UsersServices.GetUserInfo(request.UserName);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");
                    MailServices.SendEmail(request.Body, request.Subject, user.Email);
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
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "User data not found";
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
    }
}

