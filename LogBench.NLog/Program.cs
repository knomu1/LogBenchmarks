using LogBenchmarks.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog;

var builder = Host.CreateApplicationBuilder(args);

// NLog 設定読み込み（NLog.config）
LogManager.Setup().LoadConfigurationFromFile("NLog.config");

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

builder.Services.AddSingleton<LogBenchmark>();

var app = builder.Build();

app.Services.GetRequiredService<LogBenchmark>().RunBenchmark();

app.Run();
