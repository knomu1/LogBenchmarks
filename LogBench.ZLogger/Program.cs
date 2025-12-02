using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;
using System.IO;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();

// ログフォルダ
var logDir = Path.Combine("logs", "zlogger");
Directory.CreateDirectory(logDir);

// ZLogger rolling file (あなたのバリアントに完全対応)
builder.Logging.AddZLoggerRollingFile(options =>
{
    // FilePathSelector を使用
    options.FilePathSelector = (dt, seq) =>
        Path.Combine(logDir, $"zlogger-{dt:yyyyMMdd}-{seq}.log");

    options.RollingInterval = RollingInterval.Day;
    options.RollingSizeKB = 1024;
});

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

app.Services.GetRequiredService<LogBenchmark>().RunBenchmark();

app.Run();
