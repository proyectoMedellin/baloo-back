using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Requests;
using SiecaAPI.Models;
using SiecaAPI.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {

        private readonly ILogger<OrganizationsController> _logger;

        public OrganizationsController(ILogger<OrganizationsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("CreateOrganization")]
        public async Task<DtoRequestResult<DtoOrganizationResp>> CreateOrganization(DtoOrganizationReq request)
        {
            DtoRequestResult<DtoOrganizationResp> response = new DtoRequestResult<DtoOrganizationResp>();
            response.CodigoRespuesta = HttpStatusCode.OK.ToString();

            try
            {
                if (!string.IsNullOrEmpty(request.Name) && !string.IsNullOrEmpty(request.CreatedBy))
                {
                    Organization org = await OrganizationServices.CreateOrganizationAsync(request.Name, request.CreatedBy);
                    response.Registros.Add(new DtoOrganizationResp(org.Id, org.Name));
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
                _logger.LogError("CreateOrganization: CreateOrganization -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return response;
            }
        }
    }
}
