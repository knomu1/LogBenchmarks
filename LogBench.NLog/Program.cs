using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// ✨ NLog を外部 XML（NLog.config）から読み込む
LogManager.Setup().LoadConfigurationFromFile("NLog.config");

// .NET の Logger を NLog に差し替え
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Logging.AddNLog();

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

// 実行してベンチマーク
app.Services.GetRequiredService<LogBenchmark>().Run(5000);

app.Run();
