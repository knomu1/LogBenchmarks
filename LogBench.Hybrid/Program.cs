using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ZLogger;
using ZLogger.Providers;
using System;
using System.IO;

var builder = Host.CreateApplicationBuilder(args);

// ---------------------------
// 1. Serilog (Console)
// ---------------------------
builder.Logging.ClearProviders();

var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.AddSerilog(serilogLogger, dispose: true);

// ---------------------------
// 2. ZLogger (Rolling File)
// ---------------------------

// まずログディレクトリ作成
var logDir = Path.Combine("logs", "zlogger");
Directory.CreateDirectory(logDir);

// FilePathSelector を使うのが正解
builder.Logging.AddZLoggerRollingFile(options =>
{
    // ローテーションファイル名を生成
    options.FilePathSelector = (dt, seq) =>
    {
        return Path.Combine(logDir, $"zlogger-{dt:yyyyMMdd}-{seq}.log");
    };

    // 日次ローテーション
    options.RollingInterval =ZLogger.Providers.RollingInterval.Day;

    // サイズローテーション（KB）
    options.RollingSizeKB = 1024;

    // NOTE:
    // UseAsync が無いバリアントなので内部でスレッドプール利用
});

// ---------------------------
// 3. DI 登録
// ---------------------------
builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

// 実行
app.Services.GetRequiredService<LogBenchmark>().Run(5000);

app.Run();
