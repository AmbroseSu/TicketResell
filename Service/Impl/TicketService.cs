using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Impl;
using Service.Response;
using System.Collections.Generic;
using System.Net;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;


namespace Service.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IImageTicketRepository _imageTicketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, ICategoryRepository ticketCategoryRepository, IPostRepository postRepository, IMapper mapper, IImageTicketRepository imageTicketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _postRepository = postRepository;
            _mapper = mapper;
            _imageTicketRepository = imageTicketRepository;
            _userRepository = userRepository;
        }
        public async Task<ResponseDTO> CreateTicketAsync(NewTicketRequest ticket)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //tim ticket category 
                IEnumerable<Category?> category = await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId && c.IsDeleted == false);

                if (category.Count() == 0)
                {
                    return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
                }
                Ticket reqTicket = _mapper.Map<Ticket>(ticket);
                reqTicket.Status = TicketStatus.PENDING;
                await _ticketRepository.SaveAsync(reqTicket);

                foreach (string imgUrl in ticket.imgList)
                {
                    ImageTicket image = new ImageTicket()
                    {
                        ImageUrl = imgUrl,
                        IsDeleted = false,
                        TicketId = reqTicket.Id

                    };
                    await _imageTicketRepository.SaveAsync(image);
                }

                TicketDTO result = _mapper.Map<TicketDTO>(reqTicket);

                // Commit transaction
                scope.Complete();

                return ResponseUtil.GetObject(result, "Ticket created successfully", HttpStatusCode.OK, null);
            }
        }

        public async Task<ResponseDTO> DeleteTicketAsync(int id)
        {
            IEnumerable<Ticket?> result = await IsTicketValid(id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            await _ticketRepository.DeleteAsync(id);

            return ResponseUtil.GetObject("Request accepted", "Ticket Deleted successfully", HttpStatusCode.Accepted, null);
        }

        public async Task<ResponseDTO> GetTicketAsync(int id)
        {
            Ticket? result = (await _ticketRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            return await getTicketInfoResponse(result);
        }

        public async Task<ResponseDTO> GetTicketsAsync(int page, int limit)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.GetAllAsync();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "No ticket found !", HttpStatusCode.BadRequest);
            }

            return await getListTicketInforResponse(result.ToList(), page, limit);
        }

        public async Task<ResponseDTO> updateStatus(int id, string status)
        {
            Ticket? result = (await IsTicketValid(id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            if (result.Status != TicketStatus.PENDING)
            {
                return ResponseUtil.Error("Request fails", "Ticket status is not pending !", HttpStatusCode.BadRequest);
            }

            status = status.ToUpper();

            if (!TicketStatusExtensions.IsValidStatus(status))
            {
                return ResponseUtil.Error("Request fails", "Invalid status !", HttpStatusCode.BadRequest);
            }

            if (status.Equals(TicketStatus.REJECTED.ToString()))
            {
                result.Status = TicketStatus.REJECTED;
            }

            if (status.Equals(TicketStatus.VERIFIED.ToString()))
            {
                result.Status = TicketStatus.VERIFIED;
            }

            Ticket newTicket = _mapper.Map<Ticket>(result);
            await _ticketRepository.UpdateAsync(newTicket);

            return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, null);
        }

        public async Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket)
        {
            Ticket? result = (await IsTicketValid(ticket.Id)).SingleOrDefault();
            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            result.Venue = ticket.Venue;
            result.Quantity = ticket.Quantity;
            result.Status = ticket.Status;

            Ticket newTicket = _mapper.Map<Ticket>(result);
            await _ticketRepository.UpdateAsync(newTicket);
            return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, null);
        }

        private async Task<IEnumerable<Ticket>> IsTicketValid(int ticketId)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.Find(t => t.Id == ticketId && t.IsDeleted == false);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        private TicketResponse getTicketInfo(Ticket result, Post post, Category cat, User user)
        {

            TicketResponse ticketResponse = new TicketResponse(
            result.Id,
            result.Name,
            result.Price,
            result.Quantity,
                result.ExpirationDate,
            result.Venue,
                result.Status,
                result.CategoryId,
                cat.Name,
                post.Id,
                post.Title,
                post.Description,
                post.CreatedDate,
                post.Status,
                post.UserId,
                user.Email
                );

            return ticketResponse;
        }

        public async Task<ResponseDTO> getTicketByCategoryId(int id, int page, int limit)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.Find(c => c.CategoryId == id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            return await getListTicketInforResponse(result.ToList(), page, limit);

        }

        private async Task<ResponseDTO> getTicketInfoResponse(Ticket result)
        {
            Post? post = (await _postRepository.Find(p => p.TicketId == result.Id)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == result.CategoryId)).SingleOrDefault();

            if (cat == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            User? user = (await _userRepository.FindUserByIdAsync((long)post.UserId));

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }

            TicketResponse ticket = getTicketInfo(result, post, cat, user);

            List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == result.Id)).ToList();

            if (imageTickets.Count == 0)
            {
                return ResponseUtil.Error("Request fails", "Image not found !", HttpStatusCode.BadRequest);
            }

            List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);

            ticket.imageTicketDTOs = imgList;

            return ResponseUtil.GetObject(ticket, "Ticket retrieved successfully", HttpStatusCode.OK, null);
        }

        private async Task<ResponseDTO> getListTicketInforResponse(List<Ticket?> result, int page, int limit)
        {
            List<TicketResponse?> responseData = new List<TicketResponse?>();

            //Duyệt qua list ticket lấy ticket info tương ứng

            foreach (Ticket ticket in result)
            {
                Post? post = (await _postRepository.Find(p => p.TicketId == ticket.Id)).SingleOrDefault();

                if (post == null)
                {
                    return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
                }

                Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId)).SingleOrDefault();

                if (cat == null)
                {
                    return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
                }

                User? user = (await _userRepository.FindUserByIdAsync((long)post.UserId));

                if (user == null)
                {
                    return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
                }

                List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticket.Id)).ToList();

                if (imageTickets.Count == 0)
                {
                    return ResponseUtil.Error("Request fails", "Image not found !", HttpStatusCode.BadRequest);
                }

                List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
                TicketResponse ticketResponse = getTicketInfo(ticket, post, cat, user);
                ticketResponse.imageTicketDTOs = imgList;
                responseData.Add(ticketResponse);
            }

            List<TicketResponse?> data = responseData.Skip((page - 1) * limit).Take(limit).ToList();
            return ResponseUtil.GetCollection(data, "All tickets retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());
        }
    }
}
