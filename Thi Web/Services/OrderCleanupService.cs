using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;

namespace TechShop.Services
{
    public class OrderCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var cutoffTime = DateTime.Now.AddMinutes(-10);

                    // Đã sửa OrderStatus thành Status
                    var expiredOrders = await context.Orders
                        .Where(o => o.Status == "Pending" && o.OrderDate <= cutoffTime)
                        .ToListAsync(stoppingToken);

                    if (expiredOrders.Any())
                    {
                        foreach (var order in expiredOrders)
                        {
                            // Đã sửa OrderStatus thành Status
                            order.Status = "Cancelled";
                        }
                        await context.SaveChangesAsync(stoppingToken);
                        Console.WriteLine($"Đã tự động hủy {expiredOrders.Count} đơn hàng quá hạn thanh toán.");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}