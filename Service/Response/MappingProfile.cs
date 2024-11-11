using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

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
        CreateMap<NewTicket, Ticket>();
        CreateMap<updateTicketRequest, Ticket>();
        CreateMap<NewPostRequest, Post>();
        CreateMap<TicketRequest, RequestTicket>();
        CreateMap<RequestTicket, TicketRequest>();
        CreateMap<TicketRequest, TicketRequestDTO>();
        CreateMap<TicketRequestDTO, TicketRequest>();
        CreateMap<ImageTicket, ImageTicketDTO>();
        CreateMap<Post, PostDTO>();
        CreateMap<Order, OrderDTO>();
        CreateMap<OrderDTO, Order>();
        CreateMap<Feedback, FeedbackResponse>();
        CreateMap<FeedbackDTO, Feedback>();
        CreateMap<ImageFeedback, ImageFeedbackDTO>();
        CreateMap<Post, TicketResponse>();
        

    }
}