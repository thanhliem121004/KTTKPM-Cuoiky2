using System.ComponentModel.DataAnnotations;

namespace E_commerceTechnologyWebsite.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên mã giảm giá")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả cho mã giảm")]
        public string Description { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayExpired { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá giảm cho mã này!")]
     
        public int Quantity { get; set; }
        public int Status { get; set; }

    }
}
