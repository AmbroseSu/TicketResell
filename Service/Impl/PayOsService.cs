using BusinessObject;
using Microsoft.AspNetCore.Http;
using Net.payOS;
using Net.payOS.Types;
using Repository;
using Transaction = BusinessObject.Transaction;

namespace Service.Impl;

public class PayOsService : IPayOsService
{
    private readonly PayOS _payOS;
    private readonly IPlatformFeeRepository _platformFeeRepository;
    private readonly ITransactionRepository _transactionRepository;

    public PayOsService(PayOS payOs, IPlatformFeeRepository platformFeeRepository, ITransactionRepository transactionRepository)
    {
        _payOS = payOs;
        _platformFeeRepository = platformFeeRepository;
        _transactionRepository = transactionRepository;
    }

    public void PayCancel()
    {
        throw new NotImplementedException();
    }

    public void PaySuccess()
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePaymentResult> CheckOut(HttpRequest httpRequest,Transaction transaction)
    {
        try
        {
            var platformFee = (await _platformFeeRepository.Find(x => x.Id == transaction.PlatformFeeId)).SingleOrDefault();
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            float? priceFloat = transaction.Price;
            int price = priceFloat.HasValue ? (int)Math.Ceiling(priceFloat.Value) : 0;
            int? quantity = transaction.Number;
            ItemData item = new ItemData(platformFee.Name, quantity ?? 0, price);
            List<ItemData> items = new List<ItemData> { item };

            // Get the current request's base URL
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

            PaymentData paymentData = new PaymentData(
                orderCode,
                item.price*item.quantity,
                "Thanh toan don hang ",
                items,
                $"{baseUrl}/cancel",
                $"{baseUrl}/success"
            );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return createPayment;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }

    public async Task CheckPay(long orderId)
    {

        Task check = new Task(
           async () =>
            {
                int count = 0;
                while (true)
                {
                    PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                    // Console.WriteLine(paymentLinkInformation.status);
                    if (!paymentLinkInformation.status.Equals("PENDING"))
                    {
                        if (paymentLinkInformation.status.Equals("PAID"))
                        {
                            // _transactionRepository.Find(x => x.)
                            break;
                        }
                        else
                        {
                            break;      
                        }
                      
                    }
                    count++;
                    // Console.WriteLine("in loop");
                    Thread.Sleep(1000);
                    if(count == 300) break;
                }
            } 
        );
        check.Start();
        // Console.WriteLine("done task");

        await check;
    }
}