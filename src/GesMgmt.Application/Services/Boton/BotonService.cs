using GesMgmt.Application.DTOs;
using GesMgmt.Application.Interfaces;
using GesMgmt.Application.Interfaces.Boton;
using GesMgmt.Application.Logger;
using GesMgmt.Application.Validators.Boton;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static GesMgmt.Application.DTOs.Boton.BotonRequestDto;
using static GesMgmt.Application.DTOs.Boton.BotonResponseDto;

namespace GesMgmt.Application.Services.Boton
{
    public class BotonService : IBotonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationMessageService _validationMessageService;
        private readonly IAppLogger _Logger;

        public BotonService(IUnitOfWork unitOfWork, IValidationMessageService validationMessageService)
        {
            _unitOfWork = unitOfWork;
            _validationMessageService = validationMessageService;
        }

        #region "BOTONES ALFIN"

        #region "Lista de Botones"
        public async Task<ResultListDto<IEnumerable<GetGestionBotonesResponseDto>>> GetBotonesByClienteAndContratoAsync(GetGestionBotonesRequestDto gestionBotonesDto)
        {
            try
            {
                var q_repBot = await _unitOfWork.av_BotonClientes.Query();

                IEnumerable<GetGestionBotonesResponseDto> data = Enumerable.Empty<GetGestionBotonesResponseDto>();
                if (q_repBot != null)
                {
                    data = await (
                                    from s in q_repBot
                                    where s.bEstado == true
                                    && s.nId_Cliente == gestionBotonesDto.nId_Cliente
                                    && s.nId_Contrato == gestionBotonesDto.nId_Contrato
                                    select new GetGestionBotonesResponseDto
                                    {
                                        nId_Cliente = s.nId_Cliente,
                                        nId_Contrato = s.nId_Contrato,
                                        nId_Boton = s.nId_Boton,
                                        nombreBoton = s.nombreBoton,
                                        descripcionBoton = s.descripcionBoton,
                                        bEstado = s.bEstado,
                                        nCrea = s.nCrea,
                                        dFechaCrea = s.dFechaCrea,
                                        nModifica = s.nModifica,
                                        dFechaModifica = s.dFechaModifica,
                                    }
                    )
                    .ToListAsync();
                }

                var response = ResultListDto<IEnumerable<GetGestionBotonesResponseDto>>.Success(data, Const.SUCCESS_CODE, Const.SUCCESS_MESSAGE, Const.SUCCESS_MESSAGE, Const.OK_REQUEST_CODE);

                return response;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetBotonesByClienteAndContrato|DatabaseError: {ex.Message}");
                return ResultListDto<IEnumerable<GetGestionBotonesResponseDto>>.Failure(Const.ERROR_REQUEST_CODE.ToString(), "Error interno del servidor.", ex.Message, Const.ERROR_REQUEST_CODE);
            }
        }
        #endregion

        #region "Lista de Reportar Casos: + REPORTAR CASO - MAF"
        public async Task<ResultListDto<IEnumerable<GetReportarCasosResponseDto>>>GetReportarCasosAsync(GetReportarCasosRequestDto gestionZonaCartCamp)
        {
            try
            {
                var q_dcor = await _unitOfWork.av_DocxCobrarOpeResults.GetReporteCasosByClienteAndCarterasActivoAsync(gestionZonaCartCamp.nId_Cliente, gestionZonaCartCamp.nId_Cartera);
                var q_dc = await _unitOfWork.av_DocxCobrars.GetDocumentosxCobrarByClienteAndCarteraAsync(gestionZonaCartCamp.nId_Cliente, gestionZonaCartCamp.nId_Cartera);
                var q_c = await _unitOfWork.av_Carteras.GetCarteraByClienteCarteraAsync(gestionZonaCartCamp.nId_Cliente, gestionZonaCartCamp.nId_Cartera);
                var q_usu = await _unitOfWork.av_Usuarios.Query();

                var rawData = await (
                    from op in q_dcor
                    join dc in q_dc
                        on op.nId_DocxCobrar equals dc.nId_DocxCobrar
                    join usu in q_usu
                        on op.nId_UsuOpe equals usu.nId_Usuario
                        into usuarioJoin
                    from us in usuarioJoin.DefaultIfEmpty()
                    join ca in q_c
                        on op.nId_Cartera equals ca.nId_Cartera
                    where
                        op.nId_Cliente == gestionZonaCartCamp.nId_Cliente
                        && op.nId_PersDeudor == gestionZonaCartCamp.nId_PersDeudor
                        && op.nId_Cartera == gestionZonaCartCamp.nId_Cartera
                        && ca.nId_Cliente == gestionZonaCartCamp.nId_Cliente

                    orderby
                        op.dDocCobOpe_FecIni descending,
                        op.nId_DocxCobrarOpeResult descending

                    select new
                    {
                        id = op.nId_DocxCobrarOpeResult,
                        Caso = op.cDocParam01,
                        Descripcion = op.cDocOpeCobOut_Descr,
                        Cartera = ca.cCar_Nombre,
                        ApePat = us != null ? us.cUsr_ApePat : "",
                        ApeMat = us != null ? us.cUsr_ApeMat : "",
                        Nombres = us != null ? us.cUsr_Nombres : "",
                        Fecha = op.dDoc_FecIngresoGes
                    }
                ).ToListAsync();

                var data = rawData
                    .Select(x => new GetReportarCasosResponseDto
                    {
                        Id = x.id,
                        Caso = x.Caso?.ToString() ?? "",
                        Descripcion = x.Descripcion ?? "",
                        Cartera = x.Cartera?.Trim() ?? "",
                        Usuario = string.Join(" ", new[] { x.ApePat, x.ApeMat, x.Nombres }.Where(s => !string.IsNullOrWhiteSpace(s))),
                        Fec_Ingreso = x.Fecha.HasValue? x.Fecha.Value.ToString("dd/MM/yyyy HH:mm") : ""
                    })
                    .ToList();

                var response = ResultListDto<IEnumerable<GetReportarCasosResponseDto>>.Success(data, Const.SUCCESS_CODE, Const.SUCCESS_MESSAGE, Const.SUCCESS_MESSAGE, Const.OK_REQUEST_CODE);

                return response;
            }
            catch (Exception ex)
            {
                _Logger.LogError("GetReportarCasos|Error: {ex.Message}");
                return ResultListDto<IEnumerable<GetReportarCasosResponseDto>>.Failure(Const.ERROR_REQUEST_CODE.ToString(), "Error interno del servidor.", ex.Message, Const.ERROR_REQUEST_CODE);
            }
        }
        #endregion

        #region "Obtener de Reportar Casos: + REPORTAR CASO - MAF"
        public async Task<ResultDto<GetReportarCasosByIdResponseDto>> GetReportarCasosByIdAsync(int nId_DocxCobrarOpeResult)
        {
            try
            {
                GetReportarCasosByIdResponseDto data = new GetReportarCasosByIdResponseDto();
                var q_dcor = await _unitOfWork.av_DocxCobrarOpeResults.GetReporteCasosByIdAsync(nId_DocxCobrarOpeResult);
                if (q_dcor != null)
                {
                    data = new GetReportarCasosByIdResponseDto()
                    {
                        nId_DocxCobrarOpeResult = q_dcor.nId_DocxCobrarOpeResult,
                        nId_DocxCobrar = q_dcor.nId_DocxCobrar,
                        dDocCobOpe_FecIni = q_dcor.dDocCobOpe_FecIni,
                        cDocOpeCobOut_Descr = q_dcor.cDocOpeCobOut_Descr,
                        nId_UsuOpe = q_dcor.nId_UsuOpe,
                        nId_PersDeudor = q_dcor.nId_PersDeudor,
                        nId_Cartera = q_dcor.nId_Cartera,
                        nId_Cliente = q_dcor.nId_Cliente,
                        dDoc_FecActual = q_dcor.dDoc_FecActual,
                        cDocParam01 = q_dcor.cDocParam01,
                        cDocParam04 = q_dcor.cDocParam04
                    };
                }
                return ResultDto<GetReportarCasosByIdResponseDto>.Success(data, Const.SUCCESS_CODE, Const.SUCCESS_MESSAGE, Const.SUCCESS_MESSAGE, Const.OK_REQUEST_CODE);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"GetReportarCasosById|DatabaseError: {ex.Message}");
                return ResultDto<GetReportarCasosByIdResponseDto>.Failure("500", "Error interno del servidor.", ex.Message, 500);
            }
        }
        #endregion

        #region "Crear Reportar Casos: + REPORTAR CASO - MAF"
        public async Task<ResultDto<CreateReportarCasosResponseDto>> CreateReportarCasosAsync(CreateReportarCasosRequestDto reportarCasosCreateDto)
        {
            CreateReportarCasosRequestValidator validator = new CreateReportarCasosRequestValidator(_unitOfWork, _validationMessageService, reportarCasosCreateDto);

            // Validaciones
            var validationResult = await validator.Validate();

            if (validationResult.Code != Const.SUCCESS_CODE)
            {
                return validationResult;
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var q_dc = await _unitOfWork.av_DocxCobrars.GetDocxCobByClienteAndDeudorActivoAsync(reportarCasosCreateDto.nId_Cliente, reportarCasosCreateDto.nId_Cartera, reportarCasosCreateDto.nId_PersDeudor);

                var newReportarCasos = new av_DocxCobrarOpeResult
                {
                    nId_DocxCobrar = q_dc.nId_DocxCobrar,
                    dDocCobOpe_FecIni = reportarCasosCreateDto.dDocCobOpe_FecIni,
                    cDocOpeCobOut_Descr = reportarCasosCreateDto.cDocOpeCobOut_Descr,
                    nId_UsuOpe = reportarCasosCreateDto.nId_UsuOpe,
                    nId_PersDeudor = reportarCasosCreateDto.nId_PersDeudor,
                    nId_Cartera = reportarCasosCreateDto.nId_Cartera,
                    nId_Cliente = reportarCasosCreateDto.nId_Cliente,
                    dDoc_FecActual = reportarCasosCreateDto.dDoc_FecActual,
                    cDocParam01 = reportarCasosCreateDto.cDocParam01,
                    cDocParam04 = reportarCasosCreateDto.cDocParam04
                };
                var resNew = await _unitOfWork.av_DocxCobrarOpeResults.AddAsync(newReportarCasos);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                var responseDto = new CreateReportarCasosResponseDto
                {
                    nId_DocxCobrarOpeResult = resNew.nId_DocxCobrarOpeResult,
                    nId_Cliente = resNew.nId_Cliente,
                    nId_Cartera = resNew.nId_Cartera,
                    nId_DocxCobrar = resNew.nId_DocxCobrar
                };
                return ResultDto<CreateReportarCasosResponseDto>.Success(responseDto, Const.SUCCESS_CODE, Const.SUCCESS_MESSAGE, Const.SUCCESS_MESSAGE, Const.OK_REQUEST_CODE);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _Logger.LogError($"CreateReportarCasos|DatabaseError: {ex.Message}");
                return ResultDto<CreateReportarCasosResponseDto>.Failure("500", "Error interno del servidor.", ex.Message, 500);
            }
        }
        #endregion

        #region "Modificar Reportar Casos: + REPORTAR CASO - MAF"
        public async Task<ResultDto<EditReportarCasosResponseDto>> EditReportarCasosAsync(EditReportarCasosRequestDto reportarCasosUpdateDto)
        {
            EditReportarCasosRequestValidator validator = new EditReportarCasosRequestValidator(_unitOfWork, _validationMessageService, reportarCasosUpdateDto);

            var validationResult = await validator.Validate();

            if (validationResult.Code != Const.SUCCESS_CODE)
            {
                return validationResult;
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var editReportarCasos = new av_DocxCobrarOpeResult
                {
                    nId_DocxCobrar = reportarCasosUpdateDto.nId_DocxCobrar,
                    dDocCobOpe_FecIni = reportarCasosUpdateDto.dDocCobOpe_FecIni,
                    cDocOpeCobOut_Descr = reportarCasosUpdateDto.cDocOpeCobOut_Descr,
                    nId_UsuOpe = reportarCasosUpdateDto.nId_UsuOpe,
                    nId_PersDeudor = reportarCasosUpdateDto.nId_PersDeudor,
                    nId_Cartera = reportarCasosUpdateDto.nId_Cartera,
                    nId_Cliente = reportarCasosUpdateDto.nId_Cliente,
                    dDoc_FecActual = reportarCasosUpdateDto.dDoc_FecActual,
                    cDocParam01 = reportarCasosUpdateDto.cDocParam01,
                    cDocParam04 = reportarCasosUpdateDto.cDocParam04
                };
                var resEdit = await _unitOfWork.av_DocxCobrarOpeResults.UpdateAsync(editReportarCasos);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                var responseDto = new EditReportarCasosResponseDto
                {
                    nId_DocxCobrarOpeResult = resEdit.nId_DocxCobrarOpeResult,
                    nId_Cliente = resEdit.nId_Cliente,
                    nId_Cartera = resEdit.nId_Cartera,
                    nId_DocxCobrar = resEdit.nId_DocxCobrar
                };
                return ResultDto<EditReportarCasosResponseDto>.Success(responseDto, Const.SUCCESS_CODE, Const.SUCCESS_MESSAGE, Const.SUCCESS_MESSAGE, Const.OK_REQUEST_CODE);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _Logger.LogError($"UpdateReportarCasos|DatabaseError: {ex.Message}");
                return ResultDto<EditReportarCasosResponseDto>.Failure("500", "Error interno del servidor.", ex.Message, 500);
            }
        }
        #endregion
        #endregion
    }
}