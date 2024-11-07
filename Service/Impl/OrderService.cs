using System.Collections;
using System.Net;
using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class OrderService : IOrderService
{
    
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, IUserRepository userRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderStatusRepository = orderStatusRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task SaveAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> GetAllOrdersByUserId(long userId,  int page, int limit) 
    {
        try
        {
            User? user = await _userRepository.FindUserByIdAsync(userId);
            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }
            //Ticket? result = (await _ticketRepository.Find(c => c.IsDeleted == false && c.Id == ticketId)).SingleOrDefault();
            IEnumerable<Order> orders = await _orderRepository.GetAllOrdersByUserId(userId);


            
            //IEnumerable<TicketRequestDTO> ticketRequestsDto = _mapper.Map<IEnumerable<TicketRequestDTO>>(ticketRequests);
            IEnumerable<OrderDTO> ordersDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            IEnumerable<OrderDTO?> data = ordersDTO.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All tickets Request retrieved sucessfully", HttpStatusCode.OK, orders.Count(), page, limit, orders.Count());
        }
        catch (Exception e)
        {
            return ResponseUtil.Error(e.Message, "Failed!", HttpStatusCode.BadRequest);
        }
        
    }
}