namespace TechShop.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public List<TopProductItemViewModel> TopProducts { get; set; } = new();
        public List<SalesRatioItemViewModel> SalesRatio { get; set; } = new();
    }
    public class TopProductItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SoldQuantity { get; set; }
        public decimal Revenue { get; set; }
    }
    public class SalesRatioItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int SoldQuantity { get; set; }
        public double Percentage { get; set; }
    }
}