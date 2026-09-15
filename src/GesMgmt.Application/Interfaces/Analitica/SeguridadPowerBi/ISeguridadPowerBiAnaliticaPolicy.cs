using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface ISeguridadPowerBiAnaliticaPolicy
{
    bool PermitirPublicarEnWeb { get; }
}
