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
   
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        public MenuController(ILogger<MenuController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetMenuItems")]
        public async Task<IActionResult> GetMenuItems(string userName)
        {
            DtoRequestResult<List<DtoMenuResp>> response = new DtoRequestResult<List<DtoMenuResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };
            try
            {
                List<DtoMenu> menuItems = await MenuServices.GetAllMenuItemasAsync(userName);
                if (menuItems.Count == 0) throw new NoDataFoundException("No exiten registros");
                List<DtoMenu> items = menuItems.Distinct().ToList();
                List<DtoMenuResp> dtoMenuResp = new();
                foreach (DtoMenu item in items)
                {
                    dtoMenuResp.Add(new DtoMenuResp(
                        item.OrganizationId, item.MenuLabel, item.Description, item.ParentMenuId, item.Position, item.ExternalUrl, item.Route,
                        item.PermissionId, item.CreatedBy));
                }
                response.Registros.Add(new List<DtoMenuResp>(dtoMenuResp));
                return Ok(response);
            }
            catch( Exception ex )
            {
                _logger.LogError("MenuController: GetAllMenuItems -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    

    }
}
