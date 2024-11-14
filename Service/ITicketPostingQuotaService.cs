using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO.Response;

namespace Service
{
    public interface ITicketPostingQuotaService
    {
        Task<ResponseDTO> GetTotalQuota(int userId);
    }
}
