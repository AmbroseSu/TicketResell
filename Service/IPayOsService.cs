using BusinessObject;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;

namespace Service;

public interface IPayOsService
{
    void PayCancel();
    void PaySuccess();
    Task<PaymentData> CheckOut(HttpRequest httpRequest, Order order);
}