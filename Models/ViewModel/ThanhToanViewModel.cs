using E_commerceTechnologyWebsite.Models;

public class ThanhToanViewModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public List<GioHangModel> CartItems { get; set; } = new List<GioHangModel>();
    public decimal TotalAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal GrandTotal { get; set; }
}