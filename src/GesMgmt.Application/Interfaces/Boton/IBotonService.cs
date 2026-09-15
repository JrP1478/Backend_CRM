using GesMgmt.Application.DTOs;
using static GesMgmt.Application.DTOs.Boton.BotonRequestDto;
using static GesMgmt.Application.DTOs.Boton.BotonResponseDto;

namespace GesMgmt.Application.Interfaces.Boton
{
    public interface IBotonService
    {
        Task<ResultListDto<IEnumerable<GetGestionBotonesResponseDto>>> GetBotonesByClienteAndContratoAsync(GetGestionBotonesRequestDto gestionBotonesDto);
        Task<ResultListDto<IEnumerable<GetReportarCasosResponseDto>>> GetReportarCasosAsync(GetReportarCasosRequestDto gestionZonaCartCamp);
        Task<ResultDto<GetReportarCasosByIdResponseDto>> GetReportarCasosByIdAsync(int nId_DocxCobrarOpeResult);
        Task<ResultDto<CreateReportarCasosResponseDto>> CreateReportarCasosAsync(CreateReportarCasosRequestDto reportarCasosCreateDto);
        Task<ResultDto<EditReportarCasosResponseDto>> EditReportarCasosAsync(EditReportarCasosRequestDto reportarCasosUpdateDto);
    }
}