namespace DataAccess.DTO.Response;

public class ResponseDTO
{
    public Object Content { get; set; }
    public string Message { get; set; }
    public int Size { get; set; }
    public int StatusCode { get; set; }
    public MeatadataDTO MeatadataDTO { get; set; }
}