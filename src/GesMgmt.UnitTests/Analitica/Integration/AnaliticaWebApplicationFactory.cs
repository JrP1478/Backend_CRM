using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GesMgmt.UnitTests.Analitica.Integration;

internal sealed class AnaliticaWebApplicationFactory(
    Action<IServiceCollection>? configureTestServices = null)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting(
            "ConnectionStrings:CrmCobConnection",
            "Server=localhost;Database=crm_cob;Integrated Security=True;TrustServerCertificate=True;");
        builder.UseSetting(
            "ConnectionStrings:CrmAnalyticsConnection",
            "Server=localhost;Database=crm_analytics;Integrated Security=True;TrustServerCertificate=True;");

        builder.ConfigureServices(services =>
        {
            configureTestServices?.Invoke(services);
        });
    }
}
