using FluentValidation;

namespace WebAPI.Requests;

public record UpdateContatoRequest(Guid Id, string Nome, string Email, string CodigoArea, string Telefone);

public class UpdateContatoRequestValidator : AbstractValidator<UpdateContatoRequest>
{
    public UpdateContatoRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Nome)
            .NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.CodigoArea)
            .NotEmpty()
            .Length(2).WithMessage("Código de área deve possuir dois caracteres")
            .Matches(@"^\d+$").WithMessage("Código de área deve conter apenas números");
        RuleFor(x => x.Telefone)
            .NotEmpty()
            .Length(8, 9).WithMessage("Número de telefone deve possuir 8 ou 9 caracteres")
            .Matches(@"^\d+$").WithMessage("Telefone deve conter apenas números");
    }
}