using FluentValidation;
using LibraryManagement.Contracts.Responses;

namespace LibraryManagement.Api.Mappings
{
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = ex.Errors.Select(e => new ValidationResponse
                    {
                        PropertyName = e.PropertyName,
                        Message = e.ErrorMessage
                    })
                };

                await context.Response.WriteAsJsonAsync(validationFailureResponse);
            }
        }
    }
}
