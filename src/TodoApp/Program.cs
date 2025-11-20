// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

var supportedCultures = new List<CultureInfo>()
{
    new("de-DE"),
    new("en-GB"),
    new("en-US"),
    new("es-ES"),
    new("fr-FR"),
    new("ja-JP"),
};

if (builder.Environment.IsDevelopment())
{
    supportedCultures.Add(new("qps-Ploc"));
}

var options = new RequestLocalizationOptions()
{
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
};

options.SetDefaultCulture("en-GB");

builder.Services.AddSingleton(options);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddLocalization();

builder.Services.AddMvc()
                .AddViewLocalization();

builder.Services.AddDbContext<TodoContext>(
    (builder) => builder.UseInMemoryDatabase(databaseName: "TodoApp"));

var app = builder.Build();

app.UseRequestLocalization(options);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
