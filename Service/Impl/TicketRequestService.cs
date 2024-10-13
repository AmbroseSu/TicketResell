using System.Net;
using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class TicketRequestService : ITicketRequestService
{
    private readonly ITicketRequestRepository _ticketRequestRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TicketRequestService(ITicketRequestRepository ticketRequestRepository, ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper)
    {
        _ticketRequestRepository = ticketRequestRepository;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task<ResponseDTO> CreateTicketRequestAsync(RequestTicket requestTicket)
    {
        try
        {
            if (requestTicket.TicketId == null || requestTicket.UserId == null)
            {
                return ResponseUtil.Error("Request fails", "TicketID or UserID not found !", HttpStatusCode.BadRequest);
            }
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == requestTicket.TicketId)).SingleOrDefault();
            User? user = await _userRepository.FindUserByIdAsync((long)requestTicket.UserId!);
            if (ticket == null || user == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket or User not found !", HttpStatusCode.BadRequest);
            }
            TicketRequest saveTicketRequest = _mapper.Map<TicketRequest>(requestTicket);
            saveTicketRequest.TicketRequestDate = DateTime.UtcNow;
            saveTicketRequest.Status = TicketRequestStatus.WAITING;
            await _ticketRequestRepository.SaveAsync(saveTicketRequest);
            TicketRequestDTO ticketRequestDto = _mapper.Map<TicketRequestDTO>(saveTicketRequest);
            return ResponseUtil.GetObject(ticketRequestDto, "Ticket Request created successfully", HttpStatusCode.OK, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task UpdateAsync(TicketRequest ticketRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long requestId)
    {
        throw new NotImplementedException();
    }

    public Task<TicketRequest?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ResponseDTO>> FindAllTicketRequestsByTicketIdAsync(int ticketId)
    {
        try
        {
            
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}