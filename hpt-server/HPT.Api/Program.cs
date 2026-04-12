using HPT.Api.Extensions.DI;
using HTP.App.Core.Extensions.DI;
using HTP.Infrastructure.Extensions;
using HTP.Infrastructure.Extensions.DI;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddSettings();

builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddApiServices(configuration);
builder.Services.AddAppServices();
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var writeDb = services.GetRequiredService<HTP.Infrastructure.Persistence.WriteDbContext>();
    var identityDb = services.GetRequiredService<HTP.Infrastructure.Persistence.AppIdentityDbContext>();

    await writeDb.Database.MigrateAsync();
    await identityDb.Database.MigrateAsync();
}

await app.Services.SeedDataAsync(builder.Configuration);

app.Run();
