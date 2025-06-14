public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static Result<T> SuccessResult(T data) =>
        new Result<T> { Success = true, Data = data, Errors = new List<string>() };

    public static Result<T> Failure(List<string> errors) =>
        new Result<T> { Success = false, Data = default, Errors = errors };

    public static Result<T> Failure(string error) =>
        new Result<T> { Success = false, Data = default, Errors = new List<string> { error } };
}
