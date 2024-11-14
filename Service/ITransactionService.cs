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
}