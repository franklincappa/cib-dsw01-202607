namespace VentasAPI.Models.DTOs.Common;

/// <summary>
/// Respuesta genérica estándar para todos los endpoints de la API
/// </summary>
/// <typeparam name="T">Tipo de dato retornado</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Operación exitosa", int statusCode = 200)
        => new() { Success = true, Message = message, Data = data, StatusCode = statusCode };

    public static ApiResponse<T> FailResult(string message, int statusCode = 400, List<string>? errors = null)
        => new() { Success = false, Message = message, StatusCode = statusCode, Errors = errors };

    public static ApiResponse<T> NotFoundResult(string message = "Recurso no encontrado")
        => new() { Success = false, Message = message, StatusCode = 404 };
}

/// <summary>
/// Respuesta paginada genérica
/// </summary>
public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int TotalRecords { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}
