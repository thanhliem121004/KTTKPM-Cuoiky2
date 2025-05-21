using E_commerceTechnologyWebsite.Models.Momo;
using E_commerceTechnologyWebsite.Models.Order;

namespace E_commerceTechnologyWebsite.Services.Momo
{
    public interface IMomoService 
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
