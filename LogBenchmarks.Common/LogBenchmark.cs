using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LogBenchmarks.Common;

public class LogBenchmark
{
    private readonly ILogger<LogBenchmark> _logger;

    public LogBenchmark(ILogger<LogBenchmark> logger)
    {
        _logger = logger;
    }

    public void RunBenchmark()
    {
        Console.WriteLine("==== Log Benchmark ====");
        Benchmark("NoLog", 0);
        Benchmark("Small(100)", 100);
        Benchmark("Medium(10,000)", 10_000);
        Benchmark("Large(100,000)", 100_000);
    }

    private void Benchmark(string name, int iterations)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            _logger.LogInformation("Test log {Iteration}", i);
        }

        sw.Stop();

        Console.WriteLine($"{name,-12}: {iterations,8} logs  =>  {sw.ElapsedMilliseconds,6} ms");
    }
}
