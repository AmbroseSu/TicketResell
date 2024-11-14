using BusinessObject;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using Transaction = BusinessObject.Transaction;

namespace Service;

public interface IPayOsService
{
    void PayCancel();
    void PaySuccess();
    Task<CreatePaymentResult> CheckOut(HttpRequest httpRequest,Transaction transaction);
    Task<ResponseDTO> CheckPay(long orderId);

}