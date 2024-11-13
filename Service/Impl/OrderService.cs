using System.Collections;
using System.Net;
using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
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
    private readonly IPlatformFeeRepository _platformFeeRepository;
    private readonly ITransactionRepository _transactionRepository;

    public OrderService(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, IUserRepository userRepository, IMapper mapper, ITransactionRepository transactionRepository, IPlatformFeeRepository platformFeeRepository)
    {
        _orderRepository = orderRepository;
        _orderStatusRepository = orderStatusRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _platformFeeRepository = platformFeeRepository;
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
            User? user = await _userRepository.FindUserByIdAsync((int)userId);
            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }
            //Ticket? result = (await _ticketRepository.Find(c => c.IsDeleted == false && c.Id == ticketId)).SingleOrDefault();
            IEnumerable<Order> orders = await _orderRepository.GetAllOrdersByUserId(userId);


            
            //IEnumerable<TicketRequestDTO> ticketRequestsDto = _mapper.Map<IEnumerable<TicketRequestDTO>>(ticketRequests);
            IEnumerable<OrderDTO> ordersDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            IEnumerable<OrderDTO?> data = ordersDTO.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All Orders sucessfully", HttpStatusCode.OK, orders.Count(), page, limit, orders.Count());
        }
        catch (Exception e)
        {
            return ResponseUtil.Error(e.Message, "Failed!", HttpStatusCode.BadRequest);
        }
        
    }

    public async Task<Transaction> CreateTransaction(int platformFeeId, int userId, int number)
    {
        PlatformFee? platformFee = (await _platformFeeRepository.Find(c => c.Id == platformFeeId)).SingleOrDefault();
        User? user = await _userRepository.FindUserByIdAsync(userId);
        Transaction transaction = new Transaction();
        transaction.Number = number;
        transaction.Price = platformFee!.Price;
        transaction.TransactionDate = DateTime.Now.ToUniversalTime();
        transaction.PaymentMethod = PaymentMethod.QRCODE;
        transaction.Promotion = 0;
        transaction.Status = true;
        transaction.Number = number;
        transaction.PlatformFeeId = platformFeeId;
        transaction.UserId = userId;
        await _transactionRepository.SaveAsync(transaction);
        return transaction;
    }

    public async Task<ResponseDTO> GetAllOrdersByStartDayAndEndDay(string? startDay, string? endDay, int page, int limit)
    {
        try
        {
            if (startDay == null || endDay == null)
            {
                IEnumerable<Order> orders = await _orderRepository.GetAllOrders();
                IEnumerable<OrderDTO> ordersDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                IEnumerable<OrderDTO?> data = ordersDTO.Skip((page - 1) * limit).Take(limit);
                return ResponseUtil.GetCollection(data, "Successfully", HttpStatusCode.OK, orders.Count(), page, limit, orders.Count());
            }
            else
            {
                //DateTime startDateTime = DateTime.ParseExact(startDay, "dd/MM/yyyy", null).ToUniversalTime();
                DateTime startDateTime = DateTime.ParseExact(startDay, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeUniversal);
                //DateTime endDateTime = DateTime.ParseExact(endDay, "dd/MM/yyyy", null).ToUniversalTime();
                DateTime endDateTime = DateTime.ParseExact(endDay, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeUniversal);

                // Kiểm tra xem endDateTime có lớn hơn startDateTime không
                if (endDateTime < startDateTime)
                {
                    return ResponseUtil.Error("End Day must be before Start Date", "Failed", HttpStatusCode.BadRequest);
                }

                // Truy vấn đơn hàng trong khoảng thời gian cho trước
                IEnumerable<Order> orders = await _orderRepository.GetAllOrdersByStartDayAndEndDay(startDateTime, endDateTime);
                IEnumerable<OrderDTO> ordersDTO = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                IEnumerable<OrderDTO?> data = ordersDTO.Skip((page - 1) * limit).Take(limit);
                return ResponseUtil.GetCollection(data, "Successfully", HttpStatusCode.OK, orders.Count(), page, limit, orders.Count());
            }
            
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid date format. Please use dd/MM/yyyy.");
        }
        catch (Exception e)
        {
            return ResponseUtil.Error(e.Message, "Failed!", HttpStatusCode.BadRequest);
        }
    }
}