using System.Globalization;
using System.Net;
using AutoMapper;
using BusinessObject;
using BusinessObject.enums;
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

    public async Task<ResponseDTO> GetStatus(long orderCode)
    {
        var trans =(await _transactionRepository.Find(x => x.OrderCode == orderCode)).SingleOrDefault();
        if (trans != null)
        {
            return ResponseUtil.GetObject(trans.Status, "status", HttpStatusCode.OK, 0);
        }
        else
        {
            return ResponseUtil.Error("Dont have this transaction", "Null error", HttpStatusCode.BadRequest);
        }
    }

    public async Task<ResponseDTO> ChangeStatus(int orderCode, TransactionStatus transactionStatus)
    {
        Transaction? transaction = (await _transactionRepository.Find(x => x.OrderCode == orderCode)).SingleOrDefault();

        if (transactionStatus == TransactionStatus.SUCCESS)
        {
            transaction.Status = TransactionStatus.SUCCESS;
            TicketPostingQuota ticketPostingQuota = new TicketPostingQuota();
            PlatformFee platformFee =
                (await _platformFeeRepository.Find(x => x.Id == transaction.PlatformFeeId)).SingleOrDefault();
            ticketPostingQuota.Quantity = (int)platformFee.Quantity;
            ticketPostingQuota.TransactionId = transaction.Id;
            await _transactionRepository.UpdateAsync(transaction);
            await _quotaRepository.SaveAsync(ticketPostingQuota);
            
            return ResponseUtil.GetObject("ok", "ok", HttpStatusCode.OK, 0);
        }
        else
        {
            return ResponseUtil.Error("Dont have this transaction", "Null error", HttpStatusCode.BadRequest);
        }

        
    }

    public async Task<ResponseDTO> GetAllTransaction(int page,int limt)
    {
        IEnumerable<Transaction?> transactions = await _transactionRepository.Find(x => true);
        IEnumerable<TransactionDTO> transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        foreach (var trans in transactionDtos)
        {
            try
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
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        List<TransactionDTO> transactionDtosList = transactionDtos.Skip((page - 1) * limt).Take(limt).ToList();
        return ResponseUtil.GetCollection(transactionDtosList, "ok", HttpStatusCode.OK, transactions.Count(), page,
            limt, transactions.Count());
    }

    public async Task<ResponseDTO> GetAllTransactionWithDate(int page,int limit,string startDate,string endDate)
    {
        string format = "dd/MM/yyyy";

        // Kiểm tra và chuyển đổi ExpirationDate

        // Thời gian hiện tại theo LocalTime

        // Chuyển expiredDate sang UTC và gán vào reqTicket
        IEnumerable<Transaction?> transactions = null;
        if (startDate == null && endDate !=null)
        {
            // DateTime? tmp = endDate;
            if (!DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiredDate))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date format!", HttpStatusCode.BadRequest);
            }
            expiredDate = expiredDate.ToUniversalTime();

            transactions = await _transactionRepository.Find(x => x.TransactionDate <= expiredDate);
        }
        else if (startDate != null && endDate == null)
        {
            if (!DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiredDate))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date format!", HttpStatusCode.BadRequest);
            }
            expiredDate = expiredDate.ToUniversalTime();
            transactions = await _transactionRepository.Find(x => x.TransactionDate >= expiredDate);
        }
        else
        {
            if (!DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiredDate1))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date format!", HttpStatusCode.BadRequest);
            }
            expiredDate1 = expiredDate1.ToUniversalTime();
            if (!DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiredDate2))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date format!", HttpStatusCode.BadRequest);
            }
            expiredDate2 = expiredDate2.ToUniversalTime();
            transactions =
                await _transactionRepository.Find(x => x.TransactionDate <= expiredDate2 && x.TransactionDate >= expiredDate1);
        }
        
        IEnumerable<TransactionDTO> transactionDtos = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
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
        List<TransactionDTO> transactionDtosList = transactionDtos.Skip((page - 1) * limit).Take(limit).ToList();
        return ResponseUtil.GetCollection(transactionDtosList, "ok", HttpStatusCode.OK, transactions.Count(), page,
            limit, transactions.Count());
    }
}