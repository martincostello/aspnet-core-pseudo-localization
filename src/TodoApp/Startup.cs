// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using NodaTime;
using TodoApp.Data;
using TodoApp.Services;

namespace TodoApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(CreateLocalizationOptions());
            services.AddSingleton<IClock>((_) => SystemClock.Instance);
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ITodoService, TodoService>();

            services.AddLocalization();

            services.AddMvc()
                    .AddViewLocalization();

            services.AddDbContext<TodoContext>((builder) => builder.UseInMemoryDatabase(databaseName: "TodoApp"));
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider provider)
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

            app.UseRouting();
            app.UseEndpoints((p) => p.MapDefaultControllerRoute());
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
