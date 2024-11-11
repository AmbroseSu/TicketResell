using BusinessObject;
using Microsoft.AspNetCore.Http;
using Net.payOS;
using Net.payOS.Types;

namespace Service.Impl;

public class PayOsService : IPayOsService
{
    private readonly PayOS _payOS;

    public PayOsService(PayOS payOs)
    {
        _payOS = payOs;
    }

    public void PayCancel()
    {
        throw new NotImplementedException();
    }

    public void PaySuccess()
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentData> CheckOut(HttpRequest httpRequest,Order order)
    {
        try
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            int price = (int)Math.Ceiling((double)order.Ticket!.Price!);
            ItemData item = new ItemData(order.Ticket!.Name!, (int)order.Quantity!, price);
            List<ItemData> items = new List<ItemData> { item };

            // Get the current request's base URL
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

            PaymentData paymentData = new PaymentData(
                orderCode,
                item.price*item.quantity,
                "Thanh toan don hang "+orderCode,
                items,
                $"{baseUrl}/cancel",
                $"{baseUrl}/success"
            );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return paymentData;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}