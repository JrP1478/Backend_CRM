using GesMgmt.Application.DTOs;
using GesMgmt.Application.Interfaces;
using GesMgmt.Application.Interfaces.Boton;
using GesMgmt.Infraestructure.Logger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using static GesMgmt.Application.DTOs.Boton.BotonRequestDto;
using static GesMgmt.Application.DTOs.Boton.BotonResponseDto;

namespace GesMgmt.WebAPI.Controllers
{
    [ApiController]
    [Route("v1/Boton")]
    [Produces("application/json")]
    public class BotonController : ControllerBase
    {
        private readonly IBotonService _botonService;
        private readonly IValidationMessageService _validationMessageService;
        private readonly IAppLogger _Logger;
        private ValidationMessageDto _oValMsgDto;

        public BotonController(IBotonService botonService, IValidationMessageService validationMessageService, IAppLogger logger)
        {
            _botonService = botonService;
            _validationMessageService = validationMessageService;
            _oValMsgDto = new ValidationMessageDto();
            _Logger = logger;
            _Logger.LogInfo("| ** API.BS.GestionManagement ** |");
        }

        /// <summary>
        /// Obtiene la Lista de los botones por Cliente y Contrato.
        /// </summary>
        /// <remarks>
        /// Obtiene la Lista de los botones por Cliente y Contrato.
        /// </remarks>
        /// <response code="200">Obtiene la Lista de los botones Por Cliente y Contrato.</response>
        [SwaggerOperation(Summary = "[API]: Endpoint Gestion Botones Por Cliente y Contrato")]
        [HttpGet("GetBotonesByClienteAndContrato")]
        [ProducesResponseType(typeof(ResultDto<GetGestionBotonesResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGestionBotonesAsync([FromQuery] GetGestionBotonesRequestDto gestionBotonesDto)
        {
            _Logger.LogInfo($"GetBotonesByClienteAndContrato|Begin|GetBotonesByClienteAndContratoAsync|request: {JsonSerializer.Serialize(gestionBotonesDto)}");
            var result = await _botonService.GetBotonesByClienteAndContratoAsync(gestionBotonesDto);
            _Logger.LogInfo($"GetBotonesByClienteAndContratoAsync|End|GetBotonesByClienteAndContratoAsync|response: {JsonSerializer.Serialize(result)}");
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Lista de Reportar Casos: + REPORTAR CASO - CLIENTE MAF.
        /// </summary>
        /// <remarks>
        /// Lista de Reportar Casos: + REPORTAR CASO - CLIENTE MAF.
        /// </remarks>
        /// <response code="200">Lista de Reportar Casos: + REPORTAR CASO - CLIENTE MAF</response>
        [SwaggerOperation(Summary = "[API]: Endpoint Lista de Reportar Casos: + REPORTAR CASO - CLIENTE MAF")]
        [HttpGet("GetReportarCasos")]
        [ProducesResponseType(typeof(ResultListDto<IEnumerable<GetReportarCasosResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultListDto<IEnumerable<GetReportarCasosResponseDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultListDto<IEnumerable<GetReportarCasosResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportarCasosAsync([FromQuery] GetReportarCasosRequestDto reportarCasosDto)
        {
            _Logger.LogInfo($"GetReportarCasos|Begin|GetReportarCasosAsync|request: {JsonSerializer.Serialize(reportarCasosDto)}");
            var result = await _botonService.GetReportarCasosAsync(reportarCasosDto);
            _Logger.LogInfo($"GetReportarCasos|End|GetReportarCasosAsync|response: {JsonSerializer.Serialize(result)}");
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Obtiene el REPORTAR CASO - CLIENTE MAF.
        /// </summary>
        /// <remarks>
        /// Obtiene el REPORTAR CASO - CLIENTE MAF.
        /// </remarks>
        /// <response code="200">Obtiene el REPORTAR CASO - CLIENTE MAF.</response>
        [SwaggerOperation(Summary = "[API]: Endpoint Obtiene el REPORTAR CASO - CLIENTE MAF")]
        [HttpGet("{nId_DocxCobrarOpeResult}")]
        [ProducesResponseType(typeof(ResultDto<GetReportarCasosByIdResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportarCasosByIdAsync(int nId_DocxCobrarOpeResult)
        {
            _Logger.LogInfo($"GetReportarCasosById|Begin|GetReportarCasosByIdAsync|request:{nId_DocxCobrarOpeResult}");
            var result = await _botonService.GetReportarCasosByIdAsync(nId_DocxCobrarOpeResult);
            _Logger.LogInfo($"GetReportarCasosById|End|GetReportarCasosByIdAsync|response: {JsonSerializer.Serialize(result)}");
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Crear registro de REPORTAR CASO - CLIENTE MAF.
        /// </summary>
        /// <remarks>
        /// Crear registro de REPORTAR CASO - CLIENTE MAF.
        /// </remarks>
        /// <response code="200">Crear registro de REPORTAR CASO - CLIENTE MAF.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CreateReportarCasosResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReportarCasosAsync([FromBody] CreateReportarCasosRequestDto reportarCasoDto)
        {
            _Logger.LogInfo($"CreateReportarCasos|Begin|CreateReportarCasosAsync|request: {JsonSerializer.Serialize(reportarCasoDto )}");
            var result = await _botonService.CreateReportarCasosAsync(reportarCasoDto);
            _Logger.LogInfo($"CreateReportarCasos|End|CreateReportarCasosAsync|response: {JsonSerializer.Serialize(result)}");
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Editar registro de REPORTAR CASO - CLIENTE MAF.
        /// </summary>
        /// <remarks>
        /// Editar registro de REPORTAR CASO - CLIENTE MAF.
        /// </remarks>
        /// <response code="200">Editar registro de REPORTAR CASO - CLIENTE MAF.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ResultDto<EditReportarCasosResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditReportarCasosAsync([FromBody] EditReportarCasosRequestDto reportarCasoDto)
        {
            _Logger.LogInfo($"EditReportarCasos|Begin|EditReportarCasosAsync|request: {JsonSerializer.Serialize(reportarCasoDto)}");
            var result = await _botonService.EditReportarCasosAsync(reportarCasoDto);
            _Logger.LogInfo($"EditReportarCasos|End|EditReportarCasosAsync|response: {JsonSerializer.Serialize(result)}");
            return StatusCode(result.StatusCode, result);
        }
    }
}
