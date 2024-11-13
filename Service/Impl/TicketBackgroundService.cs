using BusinessObject;
using BusinessObject.enums;
using BusinessObject.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;
using Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class TicketBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<TicketBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private ITicketRepository _ticketRepository;
        private IPostRepository _postRepository;

        public TicketBackgroundService(ILogger<TicketBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting background service");
            using (var scope = _serviceProvider.CreateScope())
            {
                _ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();
                _postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
            }

            ScheduleTask();
            return Task.CompletedTask;
        }

        private void ScheduleTask()
        {
            _logger.LogInformation("Begin schedule task");
            DateTime now = DateTime.Now;
            DateTime nextRunTime = DateTime.Today; // 0 giờ sáng ngày hôm nay
            TimeSpan timeToGo = nextRunTime - now;

            if (timeToGo <= TimeSpan.Zero)
            {
                nextRunTime = nextRunTime.AddDays(1); //Nếu giờ hiện tại lớn hơn 0 giờ sáng thì chuyển sang ngày hôm sau
                timeToGo = nextRunTime - now;
            }

            // Sau 5s đầu chạy mỗi 5s
            //_timer = new Timer(test, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            // Chạy mỗi 24 giờ sau lần chạy đầu
            _timer = new Timer(RunTask, null, timeToGo, TimeSpan.FromHours(24));
        }

        private void test(object state)
        {
            _logger.LogInformation("ScheduleTask");
        }

        private async void RunTask(object state)
        {
            _logger.LogInformation("Task processing");

            DateTime tomorrow = DateTime.Today.AddDays(1).ToLocalTime();
            DateTime endOfTomorrow = tomorrow.AddDays(1).AddSeconds(-1).ToLocalTime();

            //Lấy các ticket đã hết hạn và sắp hết hạn trong 24 giờ tới
            List<Ticket?> tickets = _ticketRepository
                .Find(c => c.ExpirationDate.ToLocalTime() >= tomorrow &&
                c.ExpirationDate.ToLocalTime() <= endOfTomorrow || 
                c.ExpirationDate.ToLocalTime() < DateTime.Now).Result.OrderBy(c => c.ExpirationDate).ToList();

            if (tickets == null || !tickets.Any())
            {
                return;
            }
            //Duyệt qua các ticket đã hết hạn

            foreach (var item in tickets)
            {
                DateTime nearestExpirationTime = item.ExpirationDate.ToLocalTime();
                TimeSpan timeToGo = nearestExpirationTime - DateTime.Now;
                _logger.LogInformation("Next run time: " + timeToGo);

                if (timeToGo <= TimeSpan.Zero)
                {
                    CheckExpiration(item);
                }
                else
                {
                    _logger.LogInformation("Start waiting for the next check: " + timeToGo);
                    await Task.Delay(timeToGo);
                    CheckExpiration(item);
                }
                _logger.LogInformation("Expiration status closed update successfully, ticketId: " + item.Id);

                //tickets.Remove(item);
            }

            _logger.LogInformation("Task processed");

        }

        public void CheckExpiration(Ticket item)
        {
            if (item.ExpirationDate.ToLocalTime() <= DateTime.Now.ToLocalTime() && item.Status != TicketStatus.CLOSED)
            {
                item.Status = TicketStatus.CLOSED;
                _ticketRepository.UpdateAsync(item);
            }

            //Lấy các post ra và close post
            IEnumerable<Post?> posts = _postRepository.Find(c => c.TicketId == item.Id).Result;

            if (posts == null || !posts.Any())
            {
                return;
            }

            foreach (var post in posts)
            {
                if (post.Status != PostStatus.CLOSED)
                {
                    post.Status = PostStatus.CLOSED;
                    _postRepository.UpdateAsync(post);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop background service");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
