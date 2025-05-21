using System.ComponentModel.DataAnnotations;

namespace E_commerceTechnologyWebsite.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Hãy nhập Email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập mật khẩu")]
		public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
