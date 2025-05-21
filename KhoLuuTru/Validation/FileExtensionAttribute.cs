using System.ComponentModel.DataAnnotations;
namespace E_commerceTechnologyWebsite.KhoLuuTru.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extentions = { "jpg", "png", "jpeg" };

                bool result = extentions.Any(x=>extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Chọn ảnh có định dạng jpg hoặc png hoặc jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
