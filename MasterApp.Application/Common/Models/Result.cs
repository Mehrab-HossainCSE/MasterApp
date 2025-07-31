namespace MasterApp.Application.Common.Models;

public interface IResult
{
    bool Succeeded { get; }
    bool HasError { get; }
    bool Warninged { get; }
    IReadOnlyList<string> Messages { get; }
}

public interface IResult<T> : IResult
{
    T Data { get; }
}

public class Result : IResult
{
    private readonly List<string> _messages = new();

    public bool Succeeded { get; set; }
    public bool HasError { get; set; }
    public bool Warninged { get; set; }
    public IReadOnlyList<string> Messages => _messages;

    public void AddMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            _messages.Add(message);
    }

    public static Result Success(string message = "")
    {
        var result = new Result { Succeeded = true };
        if (!string.IsNullOrWhiteSpace(message)) result.AddMessage(message);
        return result;
    }

    public static Result Fail(string message = "")
    {
        var result = new Result { Succeeded = false, HasError = true };
        if (!string.IsNullOrWhiteSpace(message)) result.AddMessage(message);
        return result;
    }

    public static Result Warning(string message = "")
    {
        var result = new Result { Succeeded = false, Warninged = true };
        if (!string.IsNullOrWhiteSpace(message)) result.AddMessage(message);
        return result;
    }
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; set; }

    public static Result<T> Success(T data, string message = "")
    {
        var result = new Result<T> { Data = data, Succeeded = true };
        if (!string.IsNullOrWhiteSpace(message)) result.AddMessage(message);
        return result;
    }

    public static Result<T> Fail(string message)
    {
        var result = new Result<T> { Succeeded = false, HasError = true };
        if (!string.IsNullOrWhiteSpace(message)) result.AddMessage(message);
        return result;
    }
}
