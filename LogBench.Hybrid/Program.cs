using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using ZLogger;
using ZLogger.Providers;

var builder = Host.CreateApplicationBuilder(args);

// --------------------------------------------------
// 1. Serilog（JSON File + Console）
//    Console から BinaryData を除外
// --------------------------------------------------

var serilogLogDir = Path.Combine(AppContext.BaseDirectory, "logs-serilog");
Directory.CreateDirectory(serilogLogDir);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    // 🔽 サブロガーで Console 用だけフィルタする
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(le =>
            le.MessageTemplate.Text.StartsWith("BinaryData", StringComparison.Ordinal))
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        )
    )
    // JSON ファイル側はフィルタ無し（必要なら後で絞る）
    .WriteTo.File(
        path: Path.Combine(serilogLogDir, "app.json"),
        rollingInterval: Serilog.RollingInterval.Day,
        formatter: new Serilog.Formatting.Json.JsonFormatter()
    )
    .CreateLogger();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog(dispose: true);
});

// --------------------------------------------------
// 2. ZLogger（高速バイナリログ）
// --------------------------------------------------

string zloggerDir = Path.Combine(AppContext.BaseDirectory, "logs-zlogger");
Directory.CreateDirectory(zloggerDir);

builder.Logging.AddZLoggerRollingFile(options =>
{
    options.FilePathSelector = (timestamp, sequence) =>
        Path.Combine(zloggerDir, $"zlog-{timestamp:yyyyMMdd}-{sequence}.log");

    options.RollingInterval = ZLogger.Providers.RollingInterval.Day;
    options.RollingSizeKB = 1024;
    options.BackgroundBufferCapacity = 64 * 1024;
    options.FullMode = BackgroundBufferFullMode.Drop;
});

// Worker はそのまま
builder.Services.AddHostedService<BinaryLoggingWorker>();

var app = builder.Build();
app.Run();
