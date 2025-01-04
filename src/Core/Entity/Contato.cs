namespace Core.Entity;

public class Contato
{
    public Guid Id { get; set; }
    public long Offset { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string CodigoArea { get; set; }
    public required string Telefone { get; set; }
}