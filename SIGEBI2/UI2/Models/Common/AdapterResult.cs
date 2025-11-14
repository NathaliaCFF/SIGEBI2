namespace UI2.Models.Common
{
    public class AdapterResult
    {
        public bool Success { get; }
        public string Message { get; }

        protected AdapterResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static AdapterResult Ok(string message = "") => new(true, message);
        public static AdapterResult Fail(string message) => new(false, message);
    }

    public class AdapterResult<T> : AdapterResult
    {
        public T? Data { get; }

        private AdapterResult(bool success, T? data, string message)
            : base(success, message)
        {
            Data = data;
        }

        public static AdapterResult<T> Ok(T data, string message = "") => new(true, data, message);
        public static AdapterResult<T> Fail(string message) => new(false, default, message);
    }
}