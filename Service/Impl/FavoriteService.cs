using System.Net;
using BusinessObject;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class FavoriteService : IFavoriteService
{
    // private readonly ITicketRepository _ticketRepository;
    // private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ITicketService _ticketService;
    private readonly ITicketRepository _ticketRepository;
    
    public FavoriteService(ITicketRepository ticketRepository, IUserRepository userRepository, ICartItemRepository cartItemRepository, ICartRepository cartRepository, ITicketService ticketService)
    {
        // _ticketRepository = ticketRepository;
        // _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
        _ticketService = ticketService;
    }

    public async Task<ResponseDTO> AddTicketFavorite(int userId, int ticketId)
    {
        Cart? cart = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        if (cart == null)
        {
            cart = new Cart();
            cart.IsDeleted = false;
            cart.UserId = userId;
            await _cartRepository.SaveAsync(cart);
            cart = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        }
        CartItem cartItem = new CartItem();
        cartItem.TicketId = ticketId;
        cartItem.Quantity = 0;
        cartItem.CartId = cart.Id;
        await _cartItemRepository.SaveAsync(cartItem);
        // await _cartRepository.UpdateAsync(cart);
        return ResponseUtil.GetObject("ok", "ok", HttpStatusCode.OK, 0);
    }

    public async Task<ResponseDTO> GetAllFavoriteTicketByUserId(int userId)
    {
        Cart? carts = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        List<CartItem?> items = new List<CartItem?>();
        if (carts != null)
        {
            IEnumerable<CartItem?> cartItems = await _cartItemRepository.FindAsync(x => x.CartId == carts.Id);
            items = cartItems.ToList();
        }
        else
        {
            return ResponseUtil.Error("Cart null","User dont have cart",HttpStatusCode.BadRequest);
        }

        List<Ticket> tickets = new List<Ticket>();
        foreach (var cartItem in items)
        {
            Ticket ticket = (await _ticketRepository.Find(x => x.Id == cartItem.TicketId)).SingleOrDefault();
            if (ticket != null) tickets.Add(ticket);
        }

        return await _ticketService.getListTicketInforResponse(tickets, 0, tickets.Count);
    }

    public async Task<ResponseDTO> RemoveFavoriteTicket(int userId, int ticketId)
    {
        var cart = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        var cartItems =
            (await _cartItemRepository.FindAsync(x =>
                x.CartId == cart.Id && x.TicketId == ticketId && x.IsDeleted == true));
            
        if (cartItems != null)
        {

            foreach (var cartItem in cartItems)
            {
                cartItem.IsDeleted = true;
                await _cartItemRepository.UpdateAsync(cartItem);
            }

            return ResponseUtil.GetObject("Success", "ok", HttpStatusCode.OK, 0);
        }

        return ResponseUtil.Error("Id Not found", "null", HttpStatusCode.NotFound);

    }
}