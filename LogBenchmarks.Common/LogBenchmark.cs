using Microsoft.Extensions.Logging;

namespace LogBenchmarks.Common;

public class LogBenchmark
{
    private readonly ILogger<LogBenchmark> _logger;
    public LogBenchmark(ILogger<LogBenchmark> logger) => _logger = logger;

    public void Run(int repeat = 5000)
    {
        for (int i = 0; i < repeat; i++)
            _logger.LogInformation("Test {i}", i);
    }
}
