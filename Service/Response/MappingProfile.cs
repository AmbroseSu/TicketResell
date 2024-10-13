using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;

namespace Service.Response;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UpsertUserDTO>();
        CreateMap<UpsertUserDTO, User>();
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
        CreateMap<Ticket, TicketDTO>();
        CreateMap<TicketDTO, Ticket>();
        CreateMap<NewTicketRequest, Ticket>();
        CreateMap<updateTicketRequest, Ticket>();
        CreateMap<NewPostRequest, Post>();
        CreateMap<TicketRequest, RequestTicket>();
        CreateMap<RequestTicket, TicketRequest>();
        CreateMap<TicketRequest, TicketRequestDTO>();
        CreateMap<TicketRequestDTO, TicketRequest>();
    }
}