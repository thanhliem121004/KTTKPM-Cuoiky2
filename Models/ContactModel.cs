using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceTechnologyWebsite.Models
{
    public class ContactModel
    {
        [Key]

        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề website")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập bản đồ")]
        public string Map {  get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập thông tin liên hệ")]
        public string Description { get; set; }
        public string Logoimg { get; set; }
        [NotMapped]
        [FileExtensions]
        public IFormFile? ImageUpload { get; set; }
    }
}
