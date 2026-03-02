using Azure.Monitor.OpenTelemetry.AspNetCore;

using codingfreaks.OtelDemo.Logic.Core;
using codingfreaks.OtelDemo.Logic.Interfaces;
using codingfreaks.OtelDemo.Logic.OpenTelemetry;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

using System.Diagnostics.Metrics;

using MockRepo = codingfreaks.OtelDemo.Repositories.Mock;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});
var otlpName = builder.Configuration["OTEL_SERVICE_NAME"] ?? throw new InvalidOperationException("No OTEL_SERVICE_NAME configured.");
var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
Meters.Init(otlpName);
var otel = builder.Services.AddOpenTelemetry();
otel.WithMetrics(metrics =>
{
    // Metrics provider from OpenTelemetry
    metrics.AddAspNetCoreInstrumentation();
    //Our custom metrics
    metrics.AddMeter(Meters.PeopleMeter!.Name);
    // Metrics provides by ASP.NET Core in .NET 8
    metrics.AddMeter("Microsoft.AspNetCore.Hosting");
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
});
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource(Meters.ActivitySource!.Name);
});

if (!string.IsNullOrEmpty(otlpEndpoint))
{
    otel.UseOtlpExporter();
}
if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    otel.UseAzureMonitor();
}
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPeopleLogic, PeopleLogic>();
builder.Services.AddScoped<IPeopleRepository, MockRepo.PeopleRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();