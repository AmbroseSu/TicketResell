using System.Net;
using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class PlatformFeeService : IPlatformFeeService
{
    private readonly IPlatformFeeRepository _platformFeeRepository;
    private readonly IMapper _mapper;


    public PlatformFeeService(IPlatformFeeRepository platformFeeRepository, IMapper mapper)
    {
        _platformFeeRepository = platformFeeRepository;
        _mapper = mapper;
    }

    public Task CreatePlatformFee(PlatformFeeDTO platformFeeDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> GetAll(int page,int limit)
    {
        IEnumerable<PlatformFee> platformFees = (await  _platformFeeRepository.Find(x => true))!;
        IEnumerable<PlatformFeeDTO> platfomFeeDtos = _mapper.Map<IEnumerable<PlatformFeeDTO>>(platformFees);
        List<PlatformFeeDTO> listPlatformFeeDtos = platfomFeeDtos.Skip((page - 1) * limit).Take(limit).ToList();
        return ResponseUtil.GetCollection(listPlatformFeeDtos, "Get all platform fee", HttpStatusCode.OK,
            platformFees.Count(), page, limit, platfomFeeDtos.Count());
    }

    public async Task<ResponseDTO> GetByName(int page, int limit, string? query)
    {
        IEnumerable<PlatformFee> platformFees = await _platformFeeRepository.Find(x => x.Name.ToLower().Contains(query.ToLower()));
        IEnumerable<PlatformFeeDTO> platformFeeDtos = _mapper.Map<IEnumerable<PlatformFeeDTO>>(platformFees);
        List<PlatformFeeDTO> listPlatformFeeDtos = platformFeeDtos.Skip((page - 1) * limit).Take(limit).ToList();
        return ResponseUtil.GetCollection(listPlatformFeeDtos, "Search by name", HttpStatusCode.OK,
            platformFees.Count(), page, limit, platformFeeDtos.Count());
    }

    public async Task<ResponseDTO> CreatePlatformFee(PlatFormFeeRequest platFormFeeRequest)
    {
        var tmp = (await _platformFeeRepository.Find(x => x.Name.Equals(platFormFeeRequest.Name)));
        if (tmp.Any())
        {
            return ResponseUtil.Error("Duplicate name", "error", HttpStatusCode.BadRequest);
        }
        PlatformFee platformFee = new PlatformFee();
        platformFee.Name = platFormFeeRequest.Name;
        platformFee.Quantity = platFormFeeRequest.Quantity;
        platformFee.Price = platFormFeeRequest.Price;
        platformFee.IsDeleted = false;
        await _platformFeeRepository.SaveAsync(platformFee);
        return ResponseUtil.GetObject("ok", "ok", HttpStatusCode.Created,0);
    }

    public async Task<ResponseDTO> ChangeStatus(int platformFeeId)
    {
        PlatformFee? platformFee = (await _platformFeeRepository.Find(x => x.Id == platformFeeId)).SingleOrDefault();
        if (platformFee == null)
        {
            return ResponseUtil.Error("Dont have this id", "Null Error", HttpStatusCode.BadRequest);
        }
        else
        {
            platformFee.IsDeleted = !platformFee.IsDeleted;
        }

        await _platformFeeRepository.UpdateAsync(platformFee);
        return ResponseUtil.GetObject("Changed status", "ok", HttpStatusCode.OK, 0);
    }
}