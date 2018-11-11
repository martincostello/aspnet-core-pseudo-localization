// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using TodoApp.Data;
using TodoApp.Services;

namespace TodoApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(CreateLocalizationOptions());
            services.AddSingleton<IClock>((_) => SystemClock.Instance);
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ITodoService, TodoService>();

            services.AddLocalization();

            services.AddMvc()
                    .AddViewLocalization()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<TodoContext>((builder) => builder.UseInMemoryDatabase(databaseName: "TodoApp"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            app.UseRequestLocalization(provider.GetRequiredService<RequestLocalizationOptions>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private RequestLocalizationOptions CreateLocalizationOptions()
        {
            var supportedCultures = new List<CultureInfo>()
            {
                new CultureInfo("de-DE"),
                new CultureInfo("en-GB"),
                new CultureInfo("en-US"),
                new CultureInfo("es-ES"),
                new CultureInfo("fr-FR"),
                new CultureInfo("ja-JP"),
            };

            if (Environment.IsDevelopment())
            {
                supportedCultures.Add(new CultureInfo("qps-Ploc"));

                new PseudoLocalizer.Humanizer.PseudoHumanizer()
                    .Freeze()
                    .Register();
            }

            var options = new RequestLocalizationOptions()
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            return options.SetDefaultCulture("en-GB");
        }
    }
}
