using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class OperationResult<T>
{
    public T Content { get; set; }
    public OperationResultType Status { get; set; }
    public string Message { get; set; }
    

    public OperationResult(T content, OperationResultType type, string message = null)
    {
        Content = content;
        Status = type;
        Message = message;
    }

    public OperationResult(OperationResultType status)
    {
        Status = status;
    }
}