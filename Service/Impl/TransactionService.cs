using System.Net;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }


    public async Task<ResponseDTO> GetAllByuserId(int userId)
    {
        var transactions = (await _transactionRepository.Find(x => x.UserId == userId));
        return ResponseUtil.GetCollection(transactions, "Get all transaction", HttpStatusCode.OK, 0, 0, 0, 0);
    }
    
}