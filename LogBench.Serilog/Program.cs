using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// Console èoóÕÇÕíxÇ¢ÇÃÇ≈ Warning à»è„Ç…Ç∑ÇÈ
var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(serilogLogger, dispose: true);

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

app.Services.GetRequiredService<LogBenchmark>().RunBenchmark();

app.Run();
