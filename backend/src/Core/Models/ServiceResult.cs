namespace NotesBackend.Core.Models;

public class ServiceResult<T>
{
    #region Properties

    public int Code { get; init; }
    public required string Message { get; init; }
    public T? Data { get; init; } = default;
    public IList<string> Errors { get; init; } = [];

    #endregion

    #region Cosntants

    private ServiceResult()
    {

    }

    #endregion

    #region Static Methods

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>
        {
            Code = 200,
            Message = "Success",
            Data = data
        };
    }

    public static ServiceResult<T> Failure(string message)
    {
        return new ServiceResult<T>
        {
            Code = 500,
            Message = message
        };
    }

    public static ServiceResult<T> Failure(IList<string> errors)
    {
        return new ServiceResult<T>
        {
            Code = 500,
            Message = "Validation Errors",
            Errors = errors
        };
    }

    public static ServiceResult<T> Failure(Exception ex)
    {
        return new ServiceResult<T>
        {
            Code = 500,
            Message = ex.Message
        };
    }

    #endregion
}
