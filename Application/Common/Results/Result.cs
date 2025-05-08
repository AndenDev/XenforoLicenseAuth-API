using Application.Common.Errors;

namespace Application.Common.Results
{
    public class Result<T>
    {
        public bool Success { get; }
        public T? Data { get; }
        public ApplicationError? Error { get; }

        private Result(bool success, T? data, ApplicationError? error)
        {
            Success = success;
            Data = data;
            Error = error;
        }

        public static Result<T> Ok(T data)
            => new(true, data, null);

        public static Result<T> Fail(ApplicationError error)
            => new(false, default, error);
    }
}
