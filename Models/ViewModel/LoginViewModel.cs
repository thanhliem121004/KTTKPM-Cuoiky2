using System.ComponentModel.DataAnnotations;

namespace E_commerceTechnologyWebsite.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
		public string Username { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập mật khẩu")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
