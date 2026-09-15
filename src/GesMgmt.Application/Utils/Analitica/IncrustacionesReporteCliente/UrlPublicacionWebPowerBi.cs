using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Utils.Analitica;

public static class UrlPublicacionWebPowerBi
{
    private const string ExpectedHost = "app.powerbi.com";
    private const string ExpectedPath = "/view";

    public static bool IntentarNormalizar(
        string? value,
        out string normalizedUrl)
    {
        normalizedUrl = string.Empty;

        if (!Uri.TryCreate(value?.Trim(), UriKind.Absolute, out var uri) ||
            !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(uri.Host, ExpectedHost, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(uri.AbsolutePath, ExpectedPath, StringComparison.OrdinalIgnoreCase) ||
            !uri.IsDefaultPort ||
            !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Fragment) ||
            !TieneExactamenteUnCodigoPublicacion(uri.Query))
        {
            return false;
        }

        normalizedUrl = uri.AbsoluteUri;
        return true;
    }

    private static bool TieneExactamenteUnCodigoPublicacion(string query)
    {
        var publishCodeCount = 0;

        foreach (var part in query
                     .TrimStart('?')
                     .Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var pieces = part.Split('=', 2);

            if (pieces.Length != 2)
            {
                continue;
            }

            string key;

            try
            {
                key = Uri.UnescapeDataString(pieces[0]);
            }
            catch (UriFormatException)
            {
                return false;
            }

            if (!string.Equals(key, "r", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            publishCodeCount++;

            if (publishCodeCount > 1 || string.IsNullOrWhiteSpace(pieces[1]))
            {
                return false;
            }
        }

        return publishCodeCount == 1;
    }
}
