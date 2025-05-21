namespace E_commerceTechnologyWebsite.Models
{
	public class OrderModel
	{
        public int Id { get; set; }
        public string Order_Code { get; set; }
        public string CouponCode { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }  // Thêm trường địa chỉ
        public string PhoneNumber {  get; set; }
    }
}
