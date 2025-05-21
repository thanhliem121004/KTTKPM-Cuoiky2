using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace E_commerceTechnologyWebsite.Areas.Admin.KhoLuuTru
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587) // Cổng 587 tốt hơn cho bảo mật
            {
                EnableSsl = true, // Bật SSL để bảo mật kết nối
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("liem203000@gmail.com", "ixwmssjujxnsuesd") // Thay thế bằng thông tin email thực tế của bạn
            };

            // Cấu hình MailMessage
            var mailMessage = new MailMessage
            {
                From = new MailAddress("liem203000@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // Cho phép gửi email HTML
            };

            mailMessage.To.Add(email); // Thêm người nhận email

            try
            {
                // Gửi email
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
