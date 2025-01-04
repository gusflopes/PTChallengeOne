using FluentValidation;

namespace WebAPI.Configuration;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var validatorType = typeof(IValidator<>);
        var validatorTypes = typeof(ValidatorExtensions).Assembly
            .GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == validatorType));

        foreach (var validator in validatorTypes)
        {
            var implementedInterface = validator.GetInterfaces()
                .First(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == validatorType);
            
            services.AddScoped(implementedInterface, validator);
        }
        
        return services;
    }
    
}


