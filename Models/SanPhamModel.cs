using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_commerceTechnologyWebsite.KhoLuuTru.Validation;

namespace E_commerceTechnologyWebsite.Models
{
    public class SanPhamModel
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        public decimal Price { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục, thể loại")]
        public int TheLoaiId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu")]
        public int ThuongHieuId { get; set; }

        public virtual TheLoaiModel TheLoai { get; set; }

        public virtual ThuongHieuModel ThuongHieu { get; set; }
        public virtual ICollection<DanhGiaModel> DanhGia { get; set; }

        public string Image { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
        public bool IsActive { get; set; } = true; // Mặc định là true (còn hoạt động)
    }
}