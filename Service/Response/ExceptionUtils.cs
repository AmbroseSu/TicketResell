using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationException = FluentValidation.ValidationException;

namespace Service.Response;

public class ExceptionUtils
{
    public static readonly string DEFAULT_UNEXPECTED_MESSAGE = "Ops! Something went wrong...";

    // Lấy danh sách lỗi từ ValidationException (tương tự ConstraintViolationException trong Java)
    public static List<string> GetErrors(ValidationException exception)
    {
        return exception.Errors
            .Select(error => error.ErrorMessage)
            .ToList();
    }

    // Xử lý lỗi cho RuntimeException tương đương
    public static List<string> GetErrors(Exception exception)
    {
        return new List<string> { DEFAULT_UNEXPECTED_MESSAGE };
    }

    // Lấy lỗi từ ModelState trong ASP.NET Core (tương tự MethodArgumentNotValidException)
    public static List<string> GetErrors(ModelStateDictionary modelState)
    {
        return modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }

    // Chuyển đổi chuỗi phản hồi thành danh sách
    public static List<string> GetResponseString(string response)
    {
        return new List<string> { response };
    }

    // Chuyển đổi lỗi đơn thành danh sách lỗi
    public static List<string> GetError(string error)
    {
        return new List<string> { error };
    }
}