namespace ESourcing.Core.ResultModels;

public class Result<T>(bool isSuccess, string message, T? data, int totalCount)
    : IResult
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string Message { get; set; } = message;
    public T? Data { get; set; } = data;
    public int TotalCount { get; set; } = totalCount;

    public Result(bool isSuccess, string message, T? data = default(T))
        : this(isSuccess, message, data, 0)
    {
    }
}