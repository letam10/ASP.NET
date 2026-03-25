using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    [Route("Admin/Support/{action=Index}")]
    public class AdminSupportController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TawkDirectChatUrl = "https://tawk.to/chat/69bac14ebb7f0b1c337b2b54/1jk0o67bd";
            ViewBag.TawkDashboardUrl = "https://dashboard.tawk.to/";
            return View("~/Views/Admin/Support/Index.cshtml");
        }
    }
}
