using System.Net;
using AutoMapper;
using BusinessObject;
using BusinessObject.enums;
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
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public TicketRequestService(ITicketRequestRepository ticketRequestRepository, ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper, IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, IPostRepository postRepository)
    {
        _ticketRequestRepository = ticketRequestRepository;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _orderRepository = orderRepository;
        _orderStatusRepository = orderStatusRepository;
        _postRepository = postRepository;
    }


    public async Task<ResponseDTO> CreateTicketRequestAsync(RequestTicket requestTicket)
    {
        try
        {
            if (requestTicket.TicketId == null || requestTicket.UserId == null)
            {
                return ResponseUtil.Error("Request fails", "TicketID or UserID not found !", HttpStatusCode.BadRequest);
            }
            TicketRequest? ticketRequest = await _ticketRequestRepository.FindByTicketIdAndUserIdAsync(requestTicket.TicketId, requestTicket.UserId);
            if (ticketRequest != null && ticketRequest.Status == TicketRequestStatus.WAITING)
            {
                return ResponseUtil.Error("Request fails", "Ticket Request has exists !", HttpStatusCode.BadRequest);
            }
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == requestTicket.TicketId)).SingleOrDefault();
            User? user = await _userRepository.FindUserByIdAsync((long)requestTicket.UserId!);
            if (ticket == null || user == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket or User not found !", HttpStatusCode.BadRequest);
            }

            if (requestTicket.Quantity > ticket.Quantity)
            {
                return ResponseUtil.Error("Request fails", "Quantity is not enough", HttpStatusCode.BadRequest);
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

    public async Task<ResponseDTO> AcceptTicketRequestAsync(long requestId)
    {
        try
        {
            TicketRequest? ticketRequest = await _ticketRequestRepository.FindByIdAsync(requestId);
            
            if (ticketRequest == null )
            {
                return ResponseUtil.Error("Request fails", "Ticket Request not found !", HttpStatusCode.BadRequest);
            }

            if (ticketRequest.Status != TicketRequestStatus.WAITING)
            {
                return ResponseUtil.Error("Request fails", "Ticket has accept !", HttpStatusCode.BadRequest);
            }

            ticketRequest.Status = TicketRequestStatus.CONFIRMED;
            await _ticketRequestRepository.UpdateAsync(ticketRequest);
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == ticketRequest.TicketId)).SingleOrDefault();
            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }
            ticket.Quantity = ticket.Quantity - ticketRequest.Quantity;
            if (ticket.Quantity == 0)
            {
                List<TicketRequest> ticketRequestsReject = await _ticketRequestRepository.FindAllTicketRequestsByTicketIdAsync(ticketRequest.TicketId.Value);

                foreach (TicketRequest ticketRequestReject in ticketRequestsReject)
                {
                    if (ticketRequestReject.Id != ticketRequest.Id && ticketRequestReject.Status == TicketRequestStatus.WAITING)
                    {
                        ticketRequestReject.Status = TicketRequestStatus.REJECTED;
                        await _ticketRequestRepository.UpdateAsync(ticketRequestReject);
                    }
                }
                ticket.Status = TicketStatus.CLOSED;
                //ticket.IsDeleted = true;
                await _ticketRepository.UpdateAsync(ticket);
                //Post? post = (await _postRepository.Find(t => t.Id == ticket.Id)).SingleOrDefault();
                //if (post != null)
                //{
                    //post.IsDeleted = true;
                    //await _postRepository.UpdateAsync(post);
                //}
                //else
                //{
                    //return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
                //}
            }
            else
            {
                await _ticketRepository.UpdateAsync(ticket);
            }
            
            
            Order order = new Order();
            order.OrderDate = DateTime.UtcNow;
            order.Price = ticketRequest.Price;
            order.Quantity = ticketRequest.Quantity;
            order.Address = ticketRequest.Address;
            order.TicketId = ticketRequest.TicketId.Value;
            order.UserId = ticketRequest.UserId.Value;
            await _orderRepository.SaveAsync(order);
            OrderStatus orderStatus = new OrderStatus();
            orderStatus.Name = "Pending";
            orderStatus.Date = DateTime.UtcNow;
            orderStatus.OrderId = order.Id;
            await _orderStatusRepository.SaveAsync(orderStatus);
            TicketRequestDTO ticketRequestDto = _mapper.Map<TicketRequestDTO>(ticketRequest);
            return ResponseUtil.GetObject(ticketRequestDto, "Ticket Request created successfully", HttpStatusCode.OK, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task DeleteAsync(long requestId)
    {
        throw new NotImplementedException();
    }

    public Task<TicketRequest?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> FindAllTicketRequestsByTicketIdAsync(int ticketId, int page, int limit)
    {
        try
        {
            Ticket? result = (await _ticketRepository.Find(c => c.IsDeleted == false && c.Id == ticketId)).SingleOrDefault();
            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "TicketID not found !", HttpStatusCode.BadRequest);
            }

            List<TicketRequest> ticketRequests =
                await _ticketRequestRepository.FindAllTicketRequestsByTicketIdAsync(ticketId);
            
            IEnumerable<TicketRequestDTO> ticketRequestsDto = _mapper.Map<IEnumerable<TicketRequestDTO>>(ticketRequests);
            IEnumerable<TicketRequestDTO?> data = ticketRequestsDto.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All tickets Request retrieved sucessfully", HttpStatusCode.OK, page, limit, ticketRequestsDto.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}