using FresherMisa2026.Application;
using FresherMisa2026.Entities;
using System.Net;
using System.Text.Json;

namespace FresherMisa2026.WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Console.WriteLine("Before run middleware");
                // Pass the request to the next middleware/component
                await _next(context);
                Console.WriteLine("After run middleware");
            }
            catch (Exception ex)
            {
                // Handle the exception globally
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var userMessage = "Có lỗi xảy ra vui lòng liên hệ MISA!";
            object? data = null;

            if (exception is ValidateException validateEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                userMessage = "Dữ liệu không hợp lệ!";
                data = validateEx.Errors; // Danh sách cộng dồn lỗi của bạn đây
            }

            context.Response.StatusCode = statusCode;

            var response = new ServiceResponse
            {
                IsSuccess = false,
                Code = statusCode,
                UserMessage = userMessage,
                DevMessage = exception.Message,
                Data = data
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}

