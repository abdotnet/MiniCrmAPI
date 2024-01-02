using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Contracts
{
    public record MiniCrmResponse(bool Success = true, string Message = "operation completed successfully");

    public record MiniCrmResponse<T>(T? Data, bool Success = true, string Message = "operation completed successfully")
        : MiniCrmResponse(Success, Message);

    public record MiniCrmErrorResponse(string Message = "operation failed", object? Errors = null)
        : MiniCrmResponse(false, Message);

    public record MiniCrmODataResponse(object? Data, long? Count, string? context, bool Success = true, string Message = "operation completed successfully") : MiniCrmResponse(Success, Message);
    public record MiniCrmODataResponse<T>(T? Data, long? Count, string? context, bool Success = true, string Message = "operation completed successfully") : MiniCrmResponse(Success, Message);

    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public required string? Message { get; set; }
        public required string? Status { get; set; }
        public static async Task<ApiResponse<T>> GetResponse(T data, string? message, string? status)
        {
            if (data is null)
            {
                return new ApiResponse<T>()
                {
                    Message = message,
                    Status = status
                };
            }

            return await Task.FromResult(new ApiResponse<T>
            {
                Message = message,
                Status = status,
                Data = data

            });
        }
    }
}
