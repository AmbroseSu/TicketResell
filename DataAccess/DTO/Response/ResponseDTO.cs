namespace DataAccess.DTO.Response;

public class ResponseDTO
{
    public Object Content { get; set; }
    public string Message { get; set; }
    public List<String> Details { get; set; }
    public int StatusCode { get; set; }
    public MeatadataDTO MeatadataDTO { get; set; }
}