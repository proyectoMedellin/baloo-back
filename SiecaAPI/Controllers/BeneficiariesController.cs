using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.Models.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BeneficiariesController : ControllerBase
    {
        private readonly ILogger<BeneficiariesController> _logger;

        public BeneficiariesController(ILogger<BeneficiariesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DtoBeneficiariesCreateReq request)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                DtoBeneficiaries beneficiary = new()
                {
                    DocumentTypeId = request.DocumentTypeId,
                    DocumentNumber = request.DocumentNumber,
                    FirstName = request.FirstName,
                    OtherNames = request.OtherNames ?? string.Empty,
                    LastName = request.LastName,
                    OtherLastName = request.OtherLastName ?? string.Empty,
                    GenderId = request.GenderId,
                    BirthDate = request.BirthDate.ToUniversalTime(),
                    BirthCountryId = request.BirthCountryId,
                    BirthDepartmentId = request.BirthDepartmentId,
                    BirthCityId = request.BirthCityId,
                    RhId = request.RhId,
                    BloodTypeId = request.BloodTypeId,
                    EmergencyPhoneNumber = request.EmergencyPhoneNumber,
                    PhotoUrl = request.PhotoUrl ?? string.Empty,
                    AdressZoneId = request.AdressZoneId,
                    Adress = request.Adress ?? string.Empty,
                    Neighborhood = request.Neighborhood ?? string.Empty,
                    AdressPhoneNumber = request.AdressPhoneNumber ?? string.Empty,
                    AdressObservations = request.AdressObservations ?? string.Empty,
                    Enabled = request.Enabled,
                    CreatedBy = request.CreationUser
                };

                foreach(DtoBeneficiariesCreateReqFamilyMember dcfm in request.Family)
                {
                    beneficiary.FamilyMembers.Add(new DtoBeneficiariesFamily() {
                        DocumentTypeId = dcfm.DocumentTypeId,
                        DocumentNumber = dcfm.DocumentNumber,
                        FirstName = dcfm.FirstName,
                        OtherNames = dcfm.OtherNames ?? string.Empty,
                        LastName = dcfm.LastName,
                        OtherLastName = dcfm.OtherLastName ?? string.Empty,
                        FamilyRelationId = dcfm.FamilyRelation,
                        Attendant = dcfm.Attendant,
                        Enabled = dcfm.Enabled,
                        CreatedBy = request.CreationUser
                    });
                }

                response.Registros.Add(await BeneficiariesServices.CreateAsync(beneficiary)); 
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DtoBeneficiariesUpdateReq request)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                DtoBeneficiaries beneficiary = new()
                {
                    Id = request.Id,
                    OrganizationId = request.OrganizationId,
                    DocumentTypeId = request.DocumentTypeId,
                    DocumentNumber = request.DocumentNumber,
                    FirstName = request.FirstName,
                    OtherNames = request.OtherNames ?? string.Empty,
                    LastName = request.LastName,
                    OtherLastName = request.OtherLastName ?? string.Empty,
                    GenderId = request.GenderId,
                    BirthDate = request.BirthDate.ToUniversalTime(),
                    BirthCountryId = request.BirthCountryId,
                    BirthDepartmentId = request.BirthDepartmentId,
                    BirthCityId = request.BirthCityId,
                    RhId = request.RhId,
                    BloodTypeId = request.BloodTypeId,
                    EmergencyPhoneNumber = request.EmergencyPhoneNumber,
                    PhotoUrl = request.PhotoUrl ?? string.Empty,
                    AdressZoneId = request.AdressZoneId,
                    Adress = request.Adress ?? string.Empty,
                    Neighborhood = request.Neighborhood ?? string.Empty,
                    AdressPhoneNumber = request.AdressPhoneNumber ?? string.Empty,
                    AdressObservations = request.AdressObservations ?? string.Empty,
                    Enabled = request.Enabled,
                    ModifiedBy = request.ModificationUser
                };

                foreach (DtoBeneficiariesUpdateReqFamilyMember dcfm in request.Family)
                {
                    beneficiary.FamilyMembers.Add(new DtoBeneficiariesFamily()
                    {
                        Id = dcfm.Id ?? Guid.Empty,
                        OrganizationId = dcfm.OrganizationId,
                        BeneficiaryId = dcfm.BeneficiaryId,
                        DocumentTypeId = dcfm.DocumentTypeId,
                        DocumentNumber = dcfm.DocumentNumber,
                        FirstName = dcfm.FirstName,
                        OtherNames = dcfm.OtherNames ?? string.Empty,
                        LastName = dcfm.LastName,
                        OtherLastName = dcfm.OtherLastName ?? string.Empty,
                        FamilyRelationId = dcfm.FamilyRelation,
                        Attendant = dcfm.Attendant,
                        Enabled = dcfm.Enabled,
                        ModifiedBy = request.ModificationUser
                    });
                }

                response.Registros.Add(await BeneficiariesServices.UpdateAsync(beneficiary));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Update -> " + ex.Message);
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
                response.Registros.Add(await BeneficiariesServices.DeleteAsync(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Delete -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid(int? year, Guid? TrainingCenterId, Guid? CampusId,
            Guid? DevelopmentRoomId, string? documentNumber, string? name, bool? fEnabled,
            int? page, int? pageSize)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await BeneficiariesServices.GetAllAsync(year, TrainingCenterId, CampusId, 
                    DevelopmentRoomId, null, documentNumber, name, null, fEnabled, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetEnabledBeneficiaries")]
        public async Task<IActionResult> GetEnabledBeneficiaries(int? year, Guid? TrainingCenterId, Guid? CampusId,
            Guid? DevelopmentRoomId, Guid? documentType, string? documentNumber, string? name, string? group, 
            int? page, int? pageSize)
        {

            var uName = ControllerTools.GetRequestUserName(HttpContext);

            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await BeneficiariesServices.GetAllAsync(year, TrainingCenterId, CampusId,
                    DevelopmentRoomId, documentType, documentNumber, name, group, true, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
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

                response.Registros.Add(await BeneficiariesServices.GetById(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetAnthropometricDataById")]
        public async Task<IActionResult> GetAnthropometricDataById(Guid id, DateTime from, DateTime to)
        {
            DtoRequestResult<DtoBeneficiariesAnthropometricRecord> response = new()
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

                response.Registros = await BeneficiariesServices.GetAnthropometricDataFromBeneficiaryId(id,from,to);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: GetAnthropometricDataFromBeneficiaryId -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetEmotionsDataById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmotionsDataById(Guid id, DateTime from, DateTime to)
        {
            DtoRequestResult<DtoBeneficiariesEmotionsRecord> response = new()
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

                response.Registros = await BeneficiariesServices.GetEmotionsDataById(id, from, to);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: GetEmotionsDataById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetAssistenceDataById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAssistenceDataById(Guid id, DateTime from, DateTime to)
        {
            DtoRequestResult<DtoBeneficiariesEmotionsRecord> response = new()
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

                response.Registros = await BeneficiariesServices.GetAssistenceDataById(id, from, to);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: GetAssistenceDataById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetParmaDataByType")]
        public async Task<IActionResult> GetParmaDataByType(string type)
        {
            DtoRequestResult<DtoBeneficiariesParameters> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(type))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Param type not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                response.Registros = await BeneficiariesServices.GetBeneficiaryParameterInfoByType(type);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: GetParmaDataByType -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
