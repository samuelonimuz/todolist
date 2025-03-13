namespace Simplifyme.Taskly.Infrastructure.WebApi.Shared.Validators;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public sealed class BodyValidatorFilter<TRequest>(
    ILogger<BodyValidatorFilter<TRequest>> logger
) : IEndpointFilter where TRequest : class
{
    private readonly ILogger<BodyValidatorFilter<TRequest>> _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            _logger.LogInformation("Filtering request for {RequestType}.", typeof(TRequest).Name);

            var httpContext = context.HttpContext;
            if (httpContext.Request.Method == HttpMethods.Get || (httpContext.Request.ContentLength ?? 0) == 0)
                return await next(context);

            httpContext.Request.EnableBuffering();
            httpContext.Request.Body.Position = 0;

            var body = context.Arguments.OfType<TRequest>().FirstOrDefault()
                ?? throw new ArgumentException("Request body is null.");

            httpContext.Request.Body.Position = 0;

            var validationContext = new ValidationContext(body);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(body, validationContext, validationResults, validateAllProperties: true))
            {
                var errors = validationResults
                    .SelectMany(result => result.MemberNames.Select(memberName => new { memberName, error = result.ErrorMessage }))
                    .GroupBy(x => x.memberName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.error!).ToArray());

                Console.WriteLine($"Validation errorsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA: {errors.Count}");

                if (errors.Count > 0)
                    throw new ArgumentException("Invalid request:\n" + string.Join("\n", errors.SelectMany(e => e.Value)));
            }

            return await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while filtering the request.");
            return Results.Problem(new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request validation error",
                Detail = "One or more validation errors occurred.",
                Instance = context.HttpContext.Request.Path,
                Errors = new Dictionary<string, string[]> { { "errors", new string[] { ex.Message } } }
            });
        }
    }
}