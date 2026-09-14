using GesMgmt.Application.Utils.Analitica;

namespace GesMgmt.UnitTests.Analitica.Application;

public sealed class PowerBiPublishToWebUrlTests
{
    [Fact]
    public void TryNormalize_AcceptsPowerBiPublishToWebUrl()
    {
        var valid = UrlPublicacionWebPowerBi.IntentarNormalizar(
            "https://app.powerbi.com/view?r=abc123",
            out var normalized);

        Assert.True(valid);
        Assert.StartsWith("https://app.powerbi.com/view?", normalized);
    }

    [Theory]
    [InlineData("https://app.powerbi.com/reportEmbed?reportId=abc")]
    [InlineData("https://example.com/view?r=abc")]
    [InlineData("http://app.powerbi.com/view?r=abc")]
    [InlineData("https://app.powerbi.com/view")]
    [InlineData("https://app.powerbi.com/view?r=abc#section")]
    [InlineData("https://user:secret@app.powerbi.com/view?r=abc")]
    [InlineData("https://app.powerbi.com/view?r=abc&r=def")]
    public void TryNormalize_RejectsNonPublishUrls(string value)
    {
        Assert.False(UrlPublicacionWebPowerBi.IntentarNormalizar(value, out _));
    }
}
