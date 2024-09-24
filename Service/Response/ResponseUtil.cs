using System.Net;
using System.Text;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Newtonsoft.Json;

namespace Service.Response;

public class ResponseUtil
{
    // Phương thức trả về một đối tượng (response)
    public static ResponseDTO GetObject(object result, string message, HttpStatusCode status, List<string> details)
    {
        return new ResponseDTO
        {
            Content = result,
            Message = message,
            Details = details ?? new List<string>(),
            StatusCode = (int)status,
            MeatadataDTO = null // for a single object, metadata is not needed
        };
    }

    // Phương thức trả về một tập hợp đối tượng (collection)
    public static ResponseDTO GetCollection(object result, string message, HttpStatusCode status, int page, int limit, long count)
    {
        return new ResponseDTO
        {
            Content = result,
            Message = message,
            Details = new List<string>(),
            StatusCode = (int)status,
            MeatadataDTO = GetMeatadata(page, limit, count)
        };
    }

    // Phương thức trả về lỗi dạng chuỗi
    public static ResponseDTO Error(string error, string message, HttpStatusCode status)
    {
        return new ResponseDTO
        {
            Message = message,
            Details = new List<string> { error },
            StatusCode = (int)status,
            MeatadataDTO = null
        };
    }

    // Phương thức trả về lỗi dạng danh sách
    public static ResponseDTO ErrorList(List<string> errors, string message, HttpStatusCode status)
    {
        return new ResponseDTO
        {
            Message = message,
            Details = errors,
            StatusCode = (int)status,
            MeatadataDTO = null
        };
    }

    // Phương thức để tính toán và trả về metadata cho collection response
    private static MeatadataDTO GetMeatadata(int page, int limit, long count)
    {
        var totalPages = (int)Math.Ceiling((double)count / limit);

        return new MeatadataDTO
        {
            page = page,
            total = totalPages,
            limit = limit,
            hasNextPage = page < totalPages,
            hasPrevPage = page > 1
        };
    }
}