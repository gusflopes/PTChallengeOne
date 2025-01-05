using WebAPI.Requests;

namespace UnitTests.Validators;

[Collection(nameof(UpdateContatoValidatorTests))]
public class UpdateContatoValidatorTests
{
    private readonly UpdateContatoRequestValidator _validator = new();
    private readonly Guid _validId = Guid.NewGuid();
    private const string _validName = "João da Silva";
    private const string _validEmail = "joao@email.com";
    private const string _validAreaCode = "67";
    private const string _validTelephone = "992638484";

    [Fact]
    public void Update_Should_Accept_Valid_Request()
    {
        var request = new UpdateContatoRequest(
            _validId,
            _validName,
            _validEmail,
            _validAreaCode,
            _validTelephone
        );
        
        var validationResult = _validator.Validate(request);
        
        Assert.True(validationResult.IsValid);
    }
    
    [Fact]
    public void Update_Should_Return_Error_For_Empty_Guid()
    {
        var request = new UpdateContatoRequest(
            Guid.Empty,
            _validName,
            _validEmail,
            _validAreaCode,
            _validTelephone
        );

        var validationResult = _validator.Validate(request);
        
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, error =>
            error.PropertyName == nameof(UpdateContatoRequest.Id));
    }
    
    [Theory]
    [InlineData("", "Nome não pode ser vazio")]
    [InlineData(" ", "Nome não pode ser espaço em branco")]
    [InlineData(null, "Nome não pode ser nulo")]
    public void Update_Should_Return_Error_For_Invalid_Name(string invalidName, string scenario)
    {
        var request = new UpdateContatoRequest(
            _validId,
            invalidName,
            _validEmail,
            _validAreaCode,
            _validTelephone
        );

        var validationResult = _validator.Validate(request);
        
        Assert.False(validationResult.IsValid, scenario);
        Assert.Contains(validationResult.Errors, error =>
            error.PropertyName == nameof(UpdateContatoRequest.Nome));
    }

    [Theory]
    [InlineData("", "Email não pode ser vazio")]
    [InlineData("invalidemail", "Email deve conter @")]
    [InlineData("@invalidemail.com", "Email deve ter parte local")]
    [InlineData("invalidemail@", "Email deve ter domínio")]
    [InlineData(" ", "Email não pode ser espaço em branco")]
    [InlineData(null, "Email não pode ser nulo")]
    public void Update_Should_Return_Error_For_Invalid_Email(string invalidEmail, string scenario)
    {
        var request = new UpdateContatoRequest(
            _validId,
            _validName,
            invalidEmail,
            _validAreaCode,
            _validTelephone
        );

        var validationResult = _validator.Validate(request);
        
        Assert.False(validationResult.IsValid, scenario);
        Assert.Contains(validationResult.Errors, error =>
            error.PropertyName == nameof(UpdateContatoRequest.Email));
    }

    [Theory]
    [InlineData("", "Area Code vazio")]
    [InlineData("1", "Area Code menor do que mínimo")]
    [InlineData("123", "Area Code maior que o máximo")]
    [InlineData("ab", "Area Code deve aceitar apenas números")]
    [InlineData(" ", "Area Code não pode ser espaço em branco")]
    [InlineData(null, "Area Code não pode ser nulo")]
    public void Update_Should_Return_Error_For_Invalid_AreaCode(string invalidAreaCode, string scenario)
    {
        var request = new UpdateContatoRequest(
            _validId,
            _validName,
            _validEmail,
            invalidAreaCode,
            _validTelephone
        );

        var validationResult = _validator.Validate(request);
        
        Assert.False(validationResult.IsValid, scenario);
        Assert.Contains(validationResult.Errors, error =>
            error.PropertyName == nameof(UpdateContatoRequest.CodigoArea));
    }

    [Theory]
    [InlineData("", "Telefone vazio")]
    [InlineData("1234567", "Telefone muito curto")]
    [InlineData("1234567890", "Telefone muito longo")]
    [InlineData("9999-9999", "Telefone com caracteres inválidos")]
    [InlineData(" ", "Telefone não pode ser espaço em branco")]
    [InlineData(null, "Telefone não pode ser nulo")]
    [InlineData("abcd1234", "Telefone deve conter apenas números")]
    public void Update_Should_Return_Error_For_Invalid_Phone(string invalidPhone, string scenario)
    {
        var request = new UpdateContatoRequest(
            _validId,
            _validName,
            _validEmail,
            _validAreaCode,
            invalidPhone
        );

        var validationResult = _validator.Validate(request);
        
        Assert.False(validationResult.IsValid, scenario);
        Assert.Contains(validationResult.Errors, error =>
            error.PropertyName == nameof(UpdateContatoRequest.Telefone));
    }
}