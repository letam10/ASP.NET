using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;
using TechShop.Models;

namespace TechShop.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(string toEmail, string customerName, Order order);
        Task SendWelcomeEmailAsync(string toEmail, string fullName);
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task SendMembershipUpgradeEmailAsync(string toEmail, string fullName, string oldTier, string newTier, int currentPoints);
        Task<bool> SendAsync(string to, string subject, string htmlBody);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        private (string Server, int Port, string Username, string Password, string SenderEmail, string SenderName) GetSmtp()
        {
            string server = _config["SmtpSettings:Server"] ?? "";
            string portStr = _config["SmtpSettings:Port"] ?? "587";
            string username = _config["SmtpSettings:Username"] ?? "";
            string password = _config["SmtpSettings:Password"] ?? "";
            string senderEmail = _config["SmtpSettings:SenderEmail"] ?? "";
            string senderName = _config["SmtpSettings:SenderName"] ?? "TechShop";

            if (!int.TryParse(portStr, out int port)) port = 587;

            return (server, port, username, password, senderEmail, senderName);
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            var smtp = GetSmtp();
            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtp.Server, smtp.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtp.Username, smtp.Password);
                await client.SendAsync(message);

                _logger.LogInformation("Email sent ok. To={To}, Subject={Subject}", message.To.ToString(), message.Subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email send failed. To={To}, Subject={Subject}", message.To.ToString(), message.Subject);
                throw; // để controller bắt và không làm “mất dấu lỗi”
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
        public async Task<bool> SendAsync(string to, string subject, string htmlBody)
        {
            try
            {
                var host = _config["SmtpSettings:Host"];
                var port = int.Parse(_config["SmtpSettings:Port"] ?? "587");
                var enableSsl = bool.Parse(_config["SmtpSettings:EnableSsl"] ?? "true");
                var user = _config["SmtpSettings:UserName"];
                var pass = _config["SmtpSettings:Password"];
                var fromEmail = _config["SmtpSettings:FromEmail"];
                var fromName = _config["SmtpSettings:FromName"] ?? "TechShop";

                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress(fromName, fromEmail));
                message.To.Add(MimeKit.MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new MimeKit.BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(user, pass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent to {Email} with subject {Subject}", to, subject);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Send email failed to {Email} with subject {Subject}", to, subject);
                return false;
            }
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string fullName)
        {
            var smtp = GetSmtp();
            var safeName = WebUtility.HtmlEncode(fullName);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.SenderName, smtp.SenderEmail));
            message.To.Add(new MailboxAddress(fullName ?? "", toEmail));
            message.Subject = "Chào mừng bạn đến với TechShop!";

            message.Body = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family:Arial,sans-serif; color:#111'>
                        <h3>Xin chào {safeName},</h3>
                        <p>Cảm ơn bạn đã đăng ký tài khoản tại TechShop. Chúc bạn mua sắm vui vẻ!</p>
                    </div>"
            }.ToMessageBody();

            await SendEmailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var smtp = GetSmtp();
            var safeLink = WebUtility.HtmlEncode(resetLink);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.SenderName, smtp.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "TechShop - Khôi phục mật khẩu";

            message.Body = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family:Arial,sans-serif; color:#111'>
                        <h3>Yêu cầu đặt lại mật khẩu</h3>
                        <p>Vui lòng bấm link sau để đặt lại mật khẩu:</p>
                        <p><a href='{safeLink}'>Đặt lại mật khẩu</a></p>
                        <p>Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>
                    </div>"
            }.ToMessageBody();

            await SendEmailAsync(message);
        }

        public async Task SendMembershipUpgradeEmailAsync(string toEmail, string fullName, string oldTier, string newTier, int currentPoints)
        {
            var smtp = GetSmtp();
            var safeName = WebUtility.HtmlEncode(fullName);
            var safeOld = WebUtility.HtmlEncode(oldTier);
            var safeNew = WebUtility.HtmlEncode(newTier);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.SenderName, smtp.SenderEmail));
            message.To.Add(new MailboxAddress(fullName ?? "", toEmail));
            message.Subject = "TechShop - Nâng cấp thẻ thành viên";

            message.Body = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family:Arial,sans-serif; color:#111'>
                        <h3>Xin chào {safeName},</h3>
                        <p>Chúc mừng bạn đã được nâng cấp thẻ thành viên!</p>
                        <p><b>{safeOld}</b> ➜ <b>{safeNew}</b></p>
                        <p>Điểm hiện tại: <b>{currentPoints}</b></p>
                    </div>"
            }.ToMessageBody();

            await SendEmailAsync(message);
        }

        public async Task SendOrderConfirmationAsync(string toEmail, string customerName, Order order)
        {
            var smtp = GetSmtp();
            var safeName = WebUtility.HtmlEncode(customerName);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.SenderName, smtp.SenderEmail));
            message.To.Add(new MailboxAddress(customerName ?? "", toEmail));
            message.Subject = $"Xác nhận đơn hàng #{order.Id} từ TechShop";

            var html =
                $@"<div style='font-family:Arial,sans-serif; color:#111'>
                        <h2 style='color:#4f46e5;'>Cảm ơn bạn đã đặt hàng, {safeName}!</h2>
                        <p>Đơn hàng <b>#{order.Id}</b> ghi nhận lúc {order.OrderDate:dd/MM/yyyy HH:mm}.</p>
                        <table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse; width:100%; max-width:700px'>
                            <tr style='background:#f1f5f9'>
                                <th align='left'>Sản phẩm</th>
                                <th align='center'>Số lượng</th>
                                <th align='right'>Đơn giá</th>
                            </tr>";

            foreach (var d in order.OrderDetails)
            {
                var pname = WebUtility.HtmlEncode(d.Product?.Name ?? $"SP#{d.ProductId}");
                html += $@"
                    <tr>
                        <td>{pname}</td>
                        <td align='center'>{d.Quantity}</td>
                        <td align='right'>{d.UnitPrice:N0} ₫</td>
                    </tr>";
            }

            html += $@"
                        <tr style='font-weight:bold'>
                            <td colspan='2' align='right'>Tổng thanh toán:</td>
                            <td align='right' style='color:#dc3545'>{order.TotalAmount:N0} ₫</td>
                        </tr>
                    </table>
                    <p>TechShop sẽ liên hệ và giao hàng sớm nhất.</p>
                </div>";

            message.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();
            await SendEmailAsync(message);
        }
    }
}
