using Andreani.ARQ.WebHost.Extension;
using Api.Services;
using gestion_transportista_cron_requerimientos.Application;
using gestion_transportista_cron_requerimientos.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAndreaniWebHost(args);
builder.Services.ConfigureAndreaniWorkerServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<RequerimientosServices>();

var app = builder.Build();

app.ConfigureAndreaniWorker();

app.Run();
