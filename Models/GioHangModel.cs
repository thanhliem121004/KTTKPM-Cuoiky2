namespace E_commerceTechnologyWebsite.Models
{
	public class GioHangModel
	{
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total
		{
			get
			{
				return Quantity * Price;
			}
		}
		public string Image {  get; set; }
		public GioHangModel()
		{

		}
		public GioHangModel(SanPhamModel sanPham)
		{
			ProductId = sanPham.Id;
			ProductName = sanPham.Name;
			Price = sanPham.Price;
			Quantity = 1;
			Image = sanPham.Image;
		}
	}
}
