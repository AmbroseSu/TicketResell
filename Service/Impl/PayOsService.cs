using System.Net;
using BusinessObject;
using BusinessObject.enums;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Net.payOS;
using Net.payOS.Types;
using Repository;
using Service.Response;
using Transaction = BusinessObject.Transaction;

namespace Service.Impl;

public class PayOsService : IPayOsService
{
    private readonly PayOS _payOS;
    private readonly IPlatformFeeRepository _platformFeeRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITicketPostingQuotaRepository _quotaRepository;

    public PayOsService(PayOS payOs, IPlatformFeeRepository platformFeeRepository, ITransactionRepository transactionRepository, ITicketPostingQuotaRepository quotaRepository)
    {
        _payOS = payOs;
        _platformFeeRepository = platformFeeRepository;
        _transactionRepository = transactionRepository;
        _quotaRepository = quotaRepository;
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
            Transaction? transaction1 = (await _transactionRepository.Find(x => x.Id == transaction.Id)).SingleOrDefault();
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
            transaction1.OrderCode = orderCode;
            await _transactionRepository.UpdateAsync(transaction1);
            return createPayment;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }

    public async Task<ResponseDTO> CheckPay(long orderId)
    {
        Transaction? transaction = (await _transactionRepository.Find(x => x.OrderCode == orderId)).SingleOrDefault();
        PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
        if (!paymentLinkInformation.status.Equals("PENDING"))
        {
            if (paymentLinkInformation.status.Equals("PAID"))
            {
                transaction.Status = TransactionStatus.SUCCESS;
            }
            else
            {
                transaction.Status = TransactionStatus.CANCELED;
            }

            await _transactionRepository.UpdateAsync(transaction);
        }
      
        // int count = 0;
        // while (true)
        // {
        //     PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
        //     if (!paymentLinkInformation.status.Equals("PENDING"))
        //     {
        //         Transaction? transaction = (await _transactionRepository.Find(x => x.OrderCode == paymentLinkInformation.orderCode)).SingleOrDefault();
        //
        //         if (paymentLinkInformation.status.Equals("PAID"))
        //         { 
        //             if (transaction != null) transaction.Status = TransactionStatus.SUCCESS;
        //             TicketPostingQuota ticketPostingQuota = new TicketPostingQuota();
        //             PlatformFee platformFee =
        //                 (await _platformFeeRepository.Find(x => x.Id == transaction.PlatformFeeId)).SingleOrDefault();
        //             ticketPostingQuota.Quantity = (int)platformFee.Quantity;
        //             ticketPostingQuota.TransactionId = transaction.Id;
        //             await _quotaRepository.SaveAsync(ticketPostingQuota);
        //         }
        //         else
        //         {
        //             if (transaction != null) transaction.Status = TransactionStatus.CANCELED;
        //         }
        //
        //         await _transactionRepository.UpdateAsync(transaction!);
        //         break; // Kết thúc vòng lặp khi trạng thái không còn là "PENDING"
        //     }
        //
        //     count++;
        //     await Task.Delay(1000); // Sử dụng Task.Delay thay cho Thread.Sleep để tránh chặn luồng chính
        //
        //     if (count == 300)
        //     {
        //         break; // Thoát nếu đã chạy 300 lần (khoảng 5 phút)
        //     }
        // }
        //
        // Transaction? finalTransaction = (await _transactionRepository.Find(x => x.OrderCode == orderId)).SingleOrDefault();
        // await _payOS.cancelPaymentLink(orderId);
        // if (finalTransaction != null) finalTransaction.Status = TransactionStatus.CANCELED;
        // await _transactionRepository.UpdateAsync(finalTransaction!);
        return ResponseUtil.GetObject(transaction.Status, "Status", HttpStatusCode.OK, 0);
    }
}