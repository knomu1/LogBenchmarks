using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZLogger;

public class BinaryLoggingWorker : BackgroundService
{
    private readonly ILogger<BinaryLoggingWorker> _logger;

    public BinaryLoggingWorker(ILogger<BinaryLoggingWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rand = new Random();
        byte[] buffer = new byte[128];

        while (!stoppingToken.IsCancellationRequested)
        {
            rand.NextBytes(buffer);

            // HexDump を文字列にして ZLogger に渡す
            string hex = HexUtil.ToHex(buffer);

            _logger.ZLogInformation($"BinaryData {hex}");

            // Serilog JSON + Console
            _logger.LogInformation("Heartbeat {Timestamp}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }
}

public static class HexUtil
{
    private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();

    public static string ToHex(ReadOnlySpan<byte> data)
    {
        char[] result = new char[data.Length * 2];

        int j = 0;
        for (int i = 0; i < data.Length; i++)
        {
            byte b = data[i];
            result[j++] = HexChars[b >> 4];
            result[j++] = HexChars[b & 0xF];
        }

        return new string(result);
    }
}
