using System.Net;
using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
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
}