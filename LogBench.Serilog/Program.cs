using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// Serilog Ç Host Ç…ìùçá
builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/serilog.log")
    .CreateLogger();

builder.Logging.AddSerilog(logger, dispose: true);

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();
app.Services.GetRequiredService<LogBenchmark>().Run(5000);
app.Run();
