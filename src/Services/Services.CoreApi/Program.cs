using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;

using codingfreaks.OtelDemo.Logic.Core;
using codingfreaks.OtelDemo.Logic.Interfaces;
using codingfreaks.OtelDemo.Logic.OpenTelemetry;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using MockRepo = codingfreaks.OtelDemo.Repositories.Mock;

var builder = WebApplication.CreateBuilder(args);
var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
var otlpName = builder.Configuration["OTEL_SERVICE_NAME"]
               ?? throw new InvalidOperationException("No OTEL_SERVICE_NAME configured.");
var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
Meters.Init(otlpName);
builder.Logging.AddOpenTelemetry(logging =>
    {
        logging.IncludeFormattedMessage = true;
        logging.IncludeScopes = true;
        logging.AddAzureMonitorLogExporter(options =>
        {
            options.ConnectionString = appInsightsConnectionString;
        });
    })
    .SetMinimumLevel(LogLevel.Trace);
var otel = builder.Services.AddOpenTelemetry();
otel.ConfigureResource(resource =>
{
    resource.AddService(otlpName, serviceVersion: "1.0.0");
});
otel.WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation();
    metrics.AddMeter(Meters.PeopleMeter!.Name);
    metrics.AddMeter("Microsoft.AspNetCore.Hosting");
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
});
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource(Meters.ActivitySource!.Name);
    // Ensure that sampling is not used.
    tracing.SetSampler(new AlwaysOnSampler());
});
if (!string.IsNullOrEmpty(otlpEndpoint))
{
    otel.UseOtlpExporter();
}
if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    otel.UseAzureMonitor(amopt =>
    {
        amopt.ConnectionString = appInsightsConnectionString;
        // Ensure that metrics are set to live
        amopt.EnableLiveMetrics = true;
    });
}
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IWebLogic, WebLogic>();
builder.Services.AddScoped<IPeopleLogic, PeopleLogic>();
builder.Services.AddScoped<IPeopleRepository, MockRepo.PeopleRepository>();
builder.Services.AddScoped<OtelMiddleware>();
var app = builder.Build();
app.UseMiddleware<OtelMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();