using System.Diagnostics.CodeAnalysis;
using Core;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

[ExcludeFromCodeCoverage]
public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Offset).ValueGeneratedOnAdd();
        builder.Property(x => x.Nome).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.CodigoArea).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Telefone).IsRequired().HasMaxLength(9);
    }
}