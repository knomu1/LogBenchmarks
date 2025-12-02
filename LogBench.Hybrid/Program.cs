using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ZLogger;
using ZLogger.Providers;
using System.IO;

var builder = Host.CreateApplicationBuilder(args);

// ---------------------------
// Serilog Console（速度対策としてWarning以上）
// ---------------------------
var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(serilogLogger, dispose: true);

// ---------------------------
// ZLogger Rolling File（あなたのZLoggerに100%対応）
// ---------------------------
var logDir = Path.Combine("logs", "zlogger");
Directory.CreateDirectory(logDir);

builder.Logging.AddZLoggerRollingFile(options =>
{
    options.FilePathSelector = (dt, seq) =>
        Path.Combine(logDir, $"zlogger-{dt:yyyyMMdd}-{seq}.log");

    options.RollingInterval = ZLogger.Providers.RollingInterval.Day;
    options.RollingSizeKB = 1024;
});

// ---------------------------
// DI
// ---------------------------
builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

app.Services.GetRequiredService<LogBenchmark>().RunBenchmark();

app.Run();
