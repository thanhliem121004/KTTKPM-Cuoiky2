using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceTechnologyWebsite.Models
{
    public class DanhGiaModel
    {
        public int Id { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập comment!")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn đánh giá từ 1-5 sao!")]
        public string Rating { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ProductId")]
        public SanPhamModel Product { get; set; }
    }
}