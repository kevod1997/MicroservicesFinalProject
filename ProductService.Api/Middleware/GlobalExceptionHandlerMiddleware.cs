using System.Net;
using System.Text.Json;
using FluentValidation;

namespace ProductService.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env; // Para saber si estamos en desarrollo

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IHostEnvironment env) // Inyectar IHostEnvironment
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continuar con el pipeline si no hay excepciones
                await _next(context);
            }
            catch (Exception ex)
            {
                // Loguear la excepción SIEMPRE
                _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);

                // Preparar la respuesta de error
                context.Response.ContentType = "application/problem+json"; // Usar ProblemDetails
                var statusCode = HttpStatusCode.InternalServerError; // Por defecto
                object? responsePayload = null; // Usar object?

                switch (ex)
                {
                    case ValidationException validationException:
                        statusCode = HttpStatusCode.BadRequest;
                        // Crear un objeto para ProblemDetails de validación
                        var validationErrors = validationException.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => string.IsNullOrEmpty(g.Key) ? "General" : g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray()
                            );
                        responsePayload = new
                        {
                            title = "Error de Validación",
                            status = (int)statusCode,
                            errors = validationErrors
                            // traceId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier // Opcional
                        };
                        break;

                    case ArgumentException argumentException:
                        statusCode = HttpStatusCode.BadRequest;
                        responsePayload = new
                        {
                            title = "Argumento Inválido",
                            status = (int)statusCode,
                            // Incluir el mensaje de la excepción puede ser útil aquí
                            detail = argumentException.Message
                            // Podrías incluir el nombre del parámetro si está disponible
                            // paramName = argumentException.ParamName
                        };
                        break;


                    // --- Añadir casos para excepciones personalizadas ---
                    // case ProductNotFoundException productNotFoundEx: // Si tuvieras esta excepción
                    //     statusCode = HttpStatusCode.NotFound;
                    //     responsePayload = new
                    //     {
                    //         title = "Recurso no encontrado",
                    //         status = (int)statusCode,
                    //         detail = productNotFoundEx.Message
                    //     };
                    //     break;

                    // --- Caso General (Internal Server Error) ---
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        // Crear un objeto ProblemDetails genérico
                        responsePayload = new
                        {
                            title = "Error Interno del Servidor",
                            status = (int)statusCode,
                            // Solo incluir detalles en Desarrollo por seguridad
                            detail = _env.IsDevelopment() ? ex.ToString() : "Ha ocurrido un error inesperado en el servidor."
                        };
                        break;
                }

                context.Response.StatusCode = (int)statusCode;

                // Escribir la respuesta JSON
                if (responsePayload != null)
                {
                    var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(responsePayload, jsonOptions));
                }
            }
        }
    }
}