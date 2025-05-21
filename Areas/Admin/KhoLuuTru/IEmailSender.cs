namespace E_commerceTechnologyWebsite.Areas.Admin.KhoLuuTru
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message); //hàm gửi email
    }
}
