using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LogBenchmarks.Common;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(s =>
    {
        s.AddLogging(b => b.SetMinimumLevel(LogLevel.Information));
        s.AddSingleton<LogBenchmark>();
    }).Build();

var bench = host.Services.GetRequiredService<LogBenchmark>();
bench.Run(5000);
