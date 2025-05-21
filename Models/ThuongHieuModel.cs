using System.ComponentModel.DataAnnotations;

namespace E_commerceTechnologyWebsite.Models
{
    public class ThuongHieuModel
    {
		[Key] /*thuộc tính này đánh dấu Id là khóa chính của bảng dữ liệu tương ứng trong csdl*/
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên thương hiệu")]
		public string Name { get; set; } /*đảm bảo rằng thuộc tính Name phải có giá trị, không được để trống*/

		[Required(ErrorMessage = "Yêu cầu nhập mô tả thương hiệu")]
		public string Description { get; set; }
		public string Slug { get; set; }  

		public int Status { get; set; }
	}
}
