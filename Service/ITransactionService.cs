using System.Linq.Expressions;
using BusinessObject;
using BusinessObject.enums;
using DataAccess.DTO.Response;

namespace Service;

public interface ITransactionService
{
    Task<ResponseDTO> GetAllByuserId(int userId);
    Task<ResponseDTO> GetStatus(long orderCode);
    Task<ResponseDTO> ChangeStatus(int orderCode, TransactionStatus transactionStatus);
    Task<ResponseDTO> GetAllTransaction(int page, int limt);
    Task<ResponseDTO> GetAllTransactionWithDate(int page, int limit, string startDate, string endDate);
    Task<ResponseDTO> GettransactionById(int id);
    Task<ResponseDTO> GetToTalRevenue();
    Task<ResponseDTO> GetFiveTopTransaction();
}