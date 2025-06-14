public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResult(T data) =>
        new ApiResponse<T> { Success = true, Data = data, Errors = new List<string>() };

    public static ApiResponse<T> Failure(List<string> errors) =>
        new ApiResponse<T> { Success = false, Data = default, Errors = errors };

    public static ApiResponse<T> Failure(string error) =>
        new ApiResponse<T> { Success = false, Data = default, Errors = new List<string> { error } };
}
