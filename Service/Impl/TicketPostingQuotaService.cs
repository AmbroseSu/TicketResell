using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl
{
    public class TicketPostingQuotaService : ITicketPostingQuotaService
    {
        // private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITicketPostingQuotaRepository _quotaRepository;

        public TicketPostingQuotaService(ITransactionRepository transactionRepository, ITicketPostingQuotaRepository quotaRepository)
        {
            // _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _quotaRepository = quotaRepository;
        }

        public async Task<ResponseDTO> GetTotalQuota(int userId)
        {
            int total = 0;
            // User? user =(await _userRepository.Find(x => x.Id == userId)).SingleOrDefault();
            IEnumerable<Transaction?> transactions = await _transactionRepository.Find(x => x.UserId == userId);
            foreach (var transaction in transactions)
            {
                TicketPostingQuota ticketPostingQuota =
                    (await _quotaRepository.Find(x => x.TransactionId == transaction.Id)).SingleOrDefault();
                if (ticketPostingQuota != null) total += ticketPostingQuota.Quantity;
            }

            return ResponseUtil.GetObject(total, "total quota", HttpStatusCode.OK, 0);
        }
    }
}
