using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();

builder.Logging
    .SetMinimumLevel(LogLevel.Information)
    .AddZLoggerConsole()
    .AddZLoggerRollingFile(options =>
    {
        // ローテーションごとにファイルパスを決める
        options.FilePathSelector = (timestamp, sequenceNumber)
            => $"logs/{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log";

        // 日毎にローテーション
        options.RollingInterval = RollingInterval.Day;

        // サイズでも切りたい場合は KB 指定（不要ならコメントアウトでもOK）
        // options.RollingSizeKB = 1024;
    });

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();
app.Services.GetRequiredService<LogBenchmark>().Run(5000);
app.Run();
