using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Service.Impl;

public class CheckStatusService : BackgroundService
{
    private readonly ILogger<CheckStatusService> _logger;
    private bool _isRunning;
    private int _intervalInSeconds = 1;
    private int _maxDurationInSeconds = 300;

    public CheckStatusService(ILogger<CheckStatusService> logger)
    {
        _logger = logger;
        _isRunning = false;
    }

    public void StartService()
    {
        _isRunning = true;
    }

    public void StopService()
    {
        _isRunning = false;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_isRunning)
            {
                _logger.LogInformation("Service started at: {time}", DateTimeOffset.Now);
                
                int elapsedTime = 0;
                while (elapsedTime < _maxDurationInSeconds && _isRunning && !stoppingToken.IsCancellationRequested)
                {
                    PerformTask(); // Thực hiện công việc tại đây

                    await Task.Delay(_intervalInSeconds * 1000, stoppingToken);
                    elapsedTime += _intervalInSeconds;

                    if (CheckCondition()) // Kiểm tra điều kiện để dừng
                    {
                        _logger.LogInformation("Condition met, stopping service at: {time}", DateTimeOffset.Now);
                        _isRunning = false;
                        break;
                    }
                }

                _logger.LogInformation("Service completed or stopped at: {time}", DateTimeOffset.Now);
                _isRunning = false;
            }

            await Task.Delay(1000, stoppingToken); // Chờ 1 giây để kiểm tra lại nếu chưa kích hoạt
        }
    }

    private void PerformTask()
    {
        _logger.LogInformation("Performing task at: {time}", DateTimeOffset.Now);
        // Thực hiện tác vụ ở đây
    }

    private bool CheckCondition()
    {
        // Kiểm tra điều kiện dừng service
        return false; // Đổi thành true để dừng nếu cần
    }
}
