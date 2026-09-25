using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record CarteraFiltroCampanaOpcion(
    string Code,
    string Nombre,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    DateOnly FechaDisponibleDesde,
    DateOnly FechaDisponibleHasta);

/// <resumen>
/// Cartera de negocio autorizada para la sesión Analítica.
/// El nombre visible se resuelve en el CRM, mientras que Analítica mantiene
/// el identificador de scope (crm_client_id) como fuente de autorización.
/// </resumen>
public sealed record CarteraFiltroCarteraAlcance(int Id);
public sealed record CarteraFiltroUnidadNegocioOpcion(
    string Code,
    string Nombre);
public sealed record CarteraFiltroSubcarteraOpcion(
    long Id,
    string Nombre);
public sealed record CarteraFiltroSupervisorOpcion(
    int Id,
    string Nombre);
public sealed record SubcarteraCampanaDisponibilidad(
    long IdSubCartera,
    string CodigoCampana,
    DateOnly FechaDisponibleDesde,
    DateOnly FechaDisponibleHasta);
public sealed record CarteraSupervisorContextoDisponibilidad(
    int IdSupervisor,
    long IdSubCartera,
    string CodigoCampana,
    DateOnly FechaDisponibleDesde,
    DateOnly FechaDisponibleHasta);
public sealed record CarteraFiltroDisponibilidad(
    IReadOnlyList<SubcarteraCampanaDisponibilidad> SubPortfolioCampaigns,
    IReadOnlyList<CarteraSupervisorContextoDisponibilidad> SupervisorContexts);
public sealed record OpcionesFiltroCarteraResponse(
    DateOnly? FechaDisponibleDesde,
    DateOnly? FechaDisponibleHasta,
    DateTimeOffset? FechaActualizacion,
    CarteraFiltroCarteraAlcance Cartera,
    IReadOnlyList<CarteraFiltroUnidadNegocioOpcion> UnidadesNegocio,
    string? SelectedBusinessUnit,
    IReadOnlyList<CarteraFiltroCampanaOpcion> Campanas,
    IReadOnlyList<CarteraFiltroSubcarteraOpcion> SubPortfolios,
    IReadOnlyList<CarteraFiltroSupervisorOpcion> Supervisores,
    CarteraFiltroDisponibilidad Availability);
