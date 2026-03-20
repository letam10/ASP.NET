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
            // Vòng lặp chạy liên tục mỗi 1 phút
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Lấy thời điểm cách đây 10 phút
                    var cutoffTime = DateTime.Now.AddMinutes(-10);

                    // Tìm các đơn hàng trạng thái Pending và đã quá 10 phút
                    var expiredOrders = await context.Orders
                        .Where(o => o.OrderStatus == "Pending" && o.OrderDate <= cutoffTime)
                        .ToListAsync(stoppingToken);

                    if (expiredOrders.Any())
                    {
                        foreach (var order in expiredOrders)
                        {
                            order.OrderStatus = "Cancelled";
                            // Tùy chọn: Có thể cộng lại số lượng (Stock) vào Product ở đây
                        }
                        await context.SaveChangesAsync(stoppingToken);
                        Console.WriteLine($"Đã tự động hủy {expiredOrders.Count} đơn hàng quá hạn thanh toán.");
                    }
                }

                // Nghỉ 1 phút rồi quét tiếp
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}