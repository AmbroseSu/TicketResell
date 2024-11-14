using System.Net;
using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPlatformFeeRepository _platformFeeRepository;
    private readonly ITicketPostingQuotaRepository _quotaRepository;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IPlatformFeeRepository platformFeeRepository, ITicketPostingQuotaRepository quotaRepository)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _platformFeeRepository = platformFeeRepository;
        _quotaRepository = quotaRepository;
    }


    public async Task<ResponseDTO> GetAllByuserId(int userId)
    {
        IEnumerable<Transaction?> transactions = (await _transactionRepository.Find(x => x.UserId == userId));
        IEnumerable<TransactionDTO?> transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        foreach (var trans in transactionDtos)
        {
            PlatformFee platformFees =
                (await _platformFeeRepository.Find(x => x.Id == trans.PlatformFeeId)).SingleOrDefault();
            PlatformFeeDTO platformFeeDto = _mapper.Map<PlatformFeeDTO>(platformFees);
            trans.PlatformFeeDto = platformFeeDto;
            TicketPostingQuota? ticketPostingQuota =
                (await _quotaRepository.Find(x => x.Id == trans.TicketPostingQuotaId)).SingleOrDefault();
            if (ticketPostingQuota != null) trans.Quantity = ticketPostingQuota.Quantity;
            else
            {
                trans.Quantity = 0;
            }
        }
        return ResponseUtil.GetCollection(transactionDtos, "Get all transaction", HttpStatusCode.OK, 0, 0, 0, 0);
    }
    
}