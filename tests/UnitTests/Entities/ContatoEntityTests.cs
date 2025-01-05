using Core.Entity;

namespace UnitTests.Entities;

[Collection(nameof(ContatoEntityTests))]
public class ContatoEntityTests
{
    private readonly Guid _validId = Guid.NewGuid();
    private const string _validName = "João da Silva";
    private const string _validEmail = "joao@email.com";
    private const string _validAreaCode = "67";
    private const string _validTelephone = "992638484";
    
    [Fact]
    public void Should_Create_Contato()
    {
        var contato = new Contato
        {
            Id = _validId,
            Offset = 1,
            Nome = _validName,
            Email = _validEmail,
            CodigoArea = _validAreaCode,
            Telefone = _validTelephone
        };
        
        Assert.Equal(_validId, contato.Id);
        Assert.Equal(1, contato.Offset);
        Assert.Equal(_validName, contato.Nome);
        Assert.Equal(_validEmail, contato.Email);
        Assert.Equal(_validAreaCode, contato.CodigoArea);
        Assert.Equal(_validTelephone, contato.Telefone);
    }
    
}