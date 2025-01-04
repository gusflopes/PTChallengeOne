using FluentValidation;

namespace WebAPI.Requests;

public record CreateContatoRequest(string Nome, string Email, string CodigoArea, string Telefone);

public class CreateContatoRequestValidator : AbstractValidator<CreateContatoRequest>
{
    public CreateContatoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.CodigoArea)
            .NotEmpty()
            .Length(2);
        RuleFor(x => x.Telefone)
            .NotEmpty()
            .Length(8, 9)
            .Matches(@"^\d+$")
            .WithMessage("Telefone deve conter apenas números");
    }
}