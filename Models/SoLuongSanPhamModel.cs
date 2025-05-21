using System.ComponentModel.DataAnnotations;

namespace E_commerceTechnologyWebsite.Models
{
    public class SoLuongSanPhamModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số lượng sản phẩm")]
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
