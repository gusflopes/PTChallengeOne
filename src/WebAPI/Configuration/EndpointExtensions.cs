using FluentValidation;

namespace WebAPI.Endpoints;

public static class EndpointExtensions
{
    public static RouteHandlerBuilder WithValidator<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var validator = context.HttpContext
                .RequestServices.GetRequiredService<IValidator<TRequest>>();

            var request = context.Arguments
                .OfType<TRequest>()
                .FirstOrDefault();

            if (request != null)
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
            }

            return await next(context);
        });
    }
}