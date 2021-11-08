// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoApp;

/// <summary>
/// A class representing a factory for creating instances of the application.
/// </summary>
public class TestServerFixture : WebApplicationFactory<Services.ITodoService>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestServerFixture"/> class.
    /// </summary>
    public TestServerFixture()
        : base()
    {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    /// <summary>
    /// Clears the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    public virtual void ClearOutputHelper()
        => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = null;

    /// <summary>
    /// Sets the <see cref="ITestOutputHelper"/> to use.
    /// </summary>
    /// <param name="value">The <see cref="ITestOutputHelper"/> to use.</param>
    public virtual void SetOutputHelper(ITestOutputHelper value)
        => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = value;

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit());
}
