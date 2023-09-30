// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Text.Encodings.Web;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp;

/// <summary>
/// A class containing tests for loading resources in the website.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LocalizationTests"/> class.
/// </remarks>
/// <param name="fixture">The fixture to use.</param>
/// <param name="outputHelper">The test output helper to use.</param>
public class LocalizationTests(TestServerFixture fixture, ITestOutputHelper outputHelper) : IntegrationTest(fixture, outputHelper)
{
    [Theory]
    [InlineData("de-DE", "Dinge die zu tun sind")]
    [InlineData("en-GB", "Things To Do")]
    [InlineData("en-US", "Things To Do")]
    [InlineData("es-ES", "Cosas para hacer")]
    [InlineData("fr-FR", "Choses à faire")]
    [InlineData("ja-JP", "私は何をする必要がありますか")]
    [InlineData("qps-Ploc", "[Ţĥîñĝšẋẋ Ţöẋ Ðöẋ]")]
    public async Task Homepage_Is_Localized(string culture, string expected)
    {
        // Arrange
        string expectedHtml = Fixture.Server.Services.GetRequiredService<HtmlEncoder>().Encode(expected);

        using var client = Fixture.CreateClient();

        // Act
        using var response = await client.GetAsync($"/?culture={culture}");
        string html = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        html.ShouldContain(expectedHtml, Case.Sensitive, $"Unexpected content for {culture}: {html}");
    }
}
