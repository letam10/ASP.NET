public class TestController : Controller
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> TestMail()
    {
        await _emailService.SendAsync(
            "emailcuaban@gmail.com",
            "Test SMTP TechShop",
            "<h1>SMTP OK</h1>"
        );

        return Content("Sent");
    }
}