using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceTechnologyWebsite.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string OrderCode { get; set; }
		public long ProductId {  get; set; }
		public decimal Price { get; set; } // lấy giá hiện tại khỏi lấy giá product tránh ko đồng bộ
		public int Quantity { get; set; }

		[ForeignKey("ProductId")]
		public SanPhamModel Product { get; set; }
	}
}
