namespace Repository.Impl;

public class FavoriteService : IFavoriteService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    
    public FavoriteService(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }
}