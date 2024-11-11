
using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Impl;
using Service.Response;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;


namespace Service.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageTicketRepository _imageTicketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, ICategoryRepository ticketCategoryRepository, IMapper mapper, IImageTicketRepository imageTicketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _mapper = mapper;
            _imageTicketRepository = imageTicketRepository;
            _userRepository = userRepository;
        }
        //public async Task<ResponseDTO> CreateTicket(NewTicket ticket)
        //{
        //    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        User? user = await _userRepository.FindUserByIdAsync(ticket.UserId);

        //        if (user == null)
        //        {
        //            return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
        //        }

        //        //Create ticket
        //        //tim ticket category
        //        IEnumerable<Category?> category = await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId);

        //        if (category == null)
        //        {
        //            return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
        //        }

        //        Ticket reqTicket = _mapper.Map<Ticket>(ticket);
        //        reqTicket.Status = TicketStatus.PENDING;
        //        string format = "dd/MM/yyyy HH:mm";

        //        DateTime expiredDate = DateTime.ParseExact(ticket.ExpirationDate, format, CultureInfo.InvariantCulture);

        //        DateTime utcDateTime = expiredDate.ToUniversalTime();

        //        reqTicket.ExpirationDate = utcDateTime;
        //        await _ticketRepository.SaveAsync(reqTicket);

        //        //Commit transaction

        //        scope.Complete();
        //        return ResponseUtil.GetObject(reqTicket, "Ticket created successfully", HttpStatusCode.OK, 0);
        //    }
        //}

        //public async Task<ResponseDTO> DeleteTicketAsync(int id)
        //{
        //    Ticket? result = await IsTicketValid(id);

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    if (result.IsDeleted == true)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket already deleted !", HttpStatusCode.BadRequest);
        //    }

        //    result.IsDeleted = true;
        //    await _ticketRepository.DeleteAsync(id);
        //    return ResponseUtil.GetObject("Request accepted", "Ticket Deleted successfully", HttpStatusCode.Accepted, 0);
        //}

        //public async Task<ResponseDTO> GetTicketAsync(int id)
        //{
        //    Ticket? result = (await _ticketRepository.Find(c => c.Id == id)).SingleOrDefault();

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    return await getTicketInfoResponse(result);
        //}

        //public async Task<ResponseDTO> GetTicketsAsync(int page, int limit)
        //{
        //    IEnumerable<Ticket?> result = await _ticketRepository.GetAllAsync();

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "No ticket found !", HttpStatusCode.BadRequest);
        //    }

        //    return await getListTicketInforResponse(result.ToList(), page, limit);
        //}

        //public async Task<ResponseDTO> UpdateStatus(int id, string status)
        //{
        //    Ticket? result = (await IsTicketValid(id));

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    if (result.Status != TicketStatus.PENDING)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket status is not pending !", HttpStatusCode.BadRequest);
        //    }

        //    status = status.ToUpper();

        //    if (!TicketStatusExtensions.IsValidStatus(status))
        //    {
        //        return ResponseUtil.Error("Request fails", "Invalid status !", HttpStatusCode.BadRequest);
        //    }

        //    if (status.Equals(TicketStatus.REJECTED.ToString()))
        //    {
        //        result.Status = TicketStatus.REJECTED;
        //    }

        //    if (status.Equals(TicketStatus.VERIFIED.ToString()))
        //    {
        //        result.Status = TicketStatus.VERIFIED;
        //    }

        //    Ticket newTicket = _mapper.Map<Ticket>(result);
        //    await _ticketRepository.UpdateAsync(newTicket);

        //    return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, 0);
        //}

        //public async Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket)
        //{
        //    Ticket? result = (await IsTicketValid(ticket.Id));
        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    result.Venue = ticket.Venue;
        //    result.Quantity = ticket.Quantity;
        //    result.Status = ticket.Status;

        //    Ticket newTicket = _mapper.Map<Ticket>(result);
        //    await _ticketRepository.UpdateAsync(newTicket);
        //    return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, 0);
        //}

        //private async Task<Ticket> IsTicketValid(int ticketId)
        //{
        //    IEnumerable<Ticket?> result = await _ticketRepository.Find(t => t.Id == ticketId);

        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    return result.SingleOrDefault();
        //}

        //private TicketResponse getTicketInfo(Ticket result, Category cat, User user)
        //{

        //    TicketResponse ticketResponse = new TicketResponse(
        //    result.Id,
        //    result.Name,
        //    result.Price,
        //    result.Quantity,
        //        result.ExpirationDate,
        //    result.Venue,
        //        result.Status,
        //        result.CategoryId,
        //        cat.Name,
        //        result.PostTitle,
        //        result.PostDescription,
        //        result.CreateDate,
        //        result.UserId,
        //        user.Email
        //        );

        //    return ticketResponse;
        //}

        //public async Task<ResponseDTO> GetTicketByCategoryId(int id, int page, int limit)
        //{
        //    IEnumerable<Ticket?> result = await _ticketRepository.Find(c => c.CategoryId == id);

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    return await getListTicketInforResponse(result.ToList(), page, limit);

        //}

        //private async Task<ResponseDTO> getTicketInfoResponse(Ticket result)
        //{
        //    Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == result.CategoryId)).SingleOrDefault();

        //    if (cat == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
        //    }

        //    User? user = (await _userRepository.FindUserByIdAsync((long)result.UserId));

        //    if (user == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
        //    }

        //    TicketResponse ticket = getTicketInfo(result, cat, user);

        //    List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == result.Id)).ToList();

        //    if (imageTickets.Count != 0)
        //    {
        //        List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
        //        ticket.imageTicketDTOs = imgList;
        //    }
        //    else
        //    {
        //        ticket.imageTicketDTOs = null;

        //    }

        //    return ResponseUtil.GetObject(ticket, "Ticket retrieved successfully", HttpStatusCode.OK, 0);
        //}

        //private async Task<ResponseDTO> getListTicketInforResponse(List<Ticket?> result, int page, int limit)
        //{
        //    List<TicketResponse?> responseData = new List<TicketResponse?>();

        //    //Duyệt qua list ticket lấy ticket info tương ứng

        //        foreach (Ticket ticket in result)
        //    {

        //        Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId)).SingleOrDefault();

        //        if (cat == null)
        //        {
        //            return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
        //        }

        //        User? user = (await _userRepository.FindUserByIdAsync((long)ticket.UserId));

        //        if (user == null)
        //        {
        //            return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
        //        }

        //        List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticket.Id)).ToList();
        //        TicketResponse ticketResponse = getTicketInfo(ticket, cat, user);

        //        if (imageTickets.Count != 0)
        //        {
        //            List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
        //            ticketResponse.imageTicketDTOs = imgList;
        //        }
        //        else
        //        {
        //            ticketResponse.imageTicketDTOs = null;
        //        }

        //        responseData.Add(ticketResponse);
        //    }

        //    List<TicketResponse?> data = responseData.Skip((page - 1) * limit).Take(limit).ToList();
        //    return ResponseUtil.GetCollection(data, "All tickets retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        //}

        //public async Task<ResponseDTO> GetTicketByEmail(string email, int page, int limit)
        //{
        //    User? user = await _userRepository.FindUserByEmailAsync(email);

        //    if (user == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
        //    }

        //    IEnumerable<Ticket?> tickets = await _ticketRepository.Find(p => p.UserId == user.Id);

        //    if (tickets.Count() == 0)
        //    {
        //        return ResponseUtil.GetObject("Request accepted", "No ticket found !", HttpStatusCode.Accepted, 0);
        //    }

        //    List<Ticket?> ticketList = new List<Ticket?>();

        //    foreach (var item in tickets)
        //    {
        //        Ticket? result = (await _ticketRepository.Find(t => t.Id == item.Id)).SingleOrDefault();

        //        if (result == null)
        //        {
        //            return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //        }

        //        ticketList.Add(result);
        //    }

        //    return await getListTicketInforResponse(ticketList, page, limit);

        //}

        //public async Task<ResponseDTO> UpdateTicketImg(List<string> imgList, int ticketId)
        //{
        //    Ticket ticket = (await _ticketRepository.Find(t => t.Id == ticketId)).SingleOrDefault();

        //    if (ticket == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticketId)).ToList();

        //    if (imageTickets.Count != 0)
        //    {
        //        return ResponseUtil.Error("Request fails", "Image already exists !", HttpStatusCode.BadRequest);
        //    }

        //    foreach (string imgUrl in imgList)
        //    {
        //        ImageTicket image = new ImageTicket()
        //        {
        //            ImageUrl = imgUrl,
        //            IsDeleted = false,
        //            TicketId = ticketId

        //        };
        //        await _imageTicketRepository.SaveAsync(image);
        //    }

        //    return ResponseUtil.GetObject("Request accepted", "Image updated successfully", HttpStatusCode.Accepted, 0);
        //}
    }
}

