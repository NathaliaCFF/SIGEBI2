namespace Shared
{
    // Versión genérica: para métodos que devuelven datos
    public class OperationResult<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Ok(T data, string? message = null)
        {
            return new OperationResult<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operación exitosa."
            };
        }

        public static OperationResult<T> Fail(string message)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message
            };
        }
    }

    // Versión no genérica: para operaciones sin retorno de datos (por ejemplo Activar/Desactivar)
    public class OperationResult
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public static OperationResult Ok(string? message = null)
        {
            return new OperationResult
            {
                Success = true,
                Message = message ?? "Operación exitosa."
            };
        }

        public static OperationResult Fail(string message)
        {
            return new OperationResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
