//-----------------------------------------------------------------------
// <copyright file="RequestLogMiddleware.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Module
{
    using GroupPairing_API.Interface;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// The middleware that logs information about the request.
    /// </summary>
    public class RequestLogMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLogMiddleware" /> class.
        /// </summary>
        /// <param name="next">A function that can process an HTTP request.</param>
        /// <param name="logger">The module for writing log.</param>
        public RequestLogMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// A function that can process an HTTP request.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The module for writing log.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// InternalErrorResponse
        /// </summary>
        private static readonly string InternalErrorResponse = JsonSerializer.Serialize(new { message = "未知的錯誤" });

        /// <summary>
        /// A function that can process an HTTP request.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Http.HttpContext for the request.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 執行
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handle Exception from the request.
        /// </summary>
        /// <param name="context">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <param name="exception">Initializes a new instance of the System.Exception class.</param>
        /// <returns> Represents an asynchronous operation.</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.AddLog($"{context.Request.Path}{Environment.NewLine}{exception}", $"{DateTime.Today:yyyyMMdd}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(InternalErrorResponse);
        }
    }
}