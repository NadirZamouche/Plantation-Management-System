using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identitty
{
    public class VerificationResetService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public VerificationResetService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var delay = GetDelay();
            _timer = new Timer(ResetVerification, null, delay, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void ResetVerification(object state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var today = DateTime.Today;
            var dayOfWeek = today.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Tuesday || dayOfWeek == DayOfWeek.Thursday)
            {
                var plants = dbContext.Plants;
                foreach (var plant in plants)
                {
                    plant.Verification = false;
                }

                dbContext.SaveChanges();
            }
        }

        private TimeSpan GetDelay()
        {
            var now = DateTime.Now;
            var today = now.Date;
            var targetTime = today.AddHours(6);

            if (now >= targetTime)
            {
                targetTime = targetTime.AddDays(1);
            }

            var delay = targetTime - now;
            return delay;
        }
    }
}