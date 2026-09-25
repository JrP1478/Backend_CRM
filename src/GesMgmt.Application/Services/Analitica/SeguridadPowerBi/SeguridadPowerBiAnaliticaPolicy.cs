using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class SeguridadPowerBiAnaliticaPolicy(
    SeguridadPowerBiAnaliticaOptions options)
    : ISeguridadPowerBiAnaliticaPolicy
{
    public bool PermitirPublicarEnWeb => options.PermitirPublicarEnWeb;
}
