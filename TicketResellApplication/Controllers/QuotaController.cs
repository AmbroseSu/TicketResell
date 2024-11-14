
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class QuotaController : ControllerBase
    {
        private readonly ITicketPostingQuotaService _postingQuota;

        public QuotaController(ITicketPostingQuotaService postingQuota)
        {
            _postingQuota = postingQuota;
        }

        [HttpGet("get-total-quota/{userId}")]
        public async Task<ResponseDTO> GetTotalQuota(int userId)
        {
            return await _postingQuota.GetTotalQuota(userId);
        }
    }

}