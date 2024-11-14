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
    
    public FavoriteService(ITicketRepository ticketRepository, IUserRepository userRepository, ICartItemRepository cartItemRepository, ICartRepository cartRepository)
    {
        // _ticketRepository = ticketRepository;
        // _userRepository = userRepository;
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
    }

    public async Task<ResponseDTO> AddTicketFavorite(int userId, int ticketId)
    {
        Cart? cart = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        if (cart == null)
        {
            cart = new Cart();
            cart.IsDeleted = false;
            cart.UserId = userId;
            cart.CartItems = new List<CartItem>();
            await _cartRepository.SaveAsync(cart);
            cart = (await _cartRepository.FindAsync(x => x.UserId == userId)).SingleOrDefault();
        }
        CartItem cartItem = new CartItem();
        cartItem.TicketId = ticketId;
        cartItem.Quantity = 0;
        cart.CartItems.Add(cartItem);
        await _cartRepository.UpdateAsync(cart);
        return ResponseUtil.GetObject("ok", "ok", HttpStatusCode.OK, 0);
    }
}