using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teste_webmotors.Model;

namespace teste_webmotors.Models.Mappings
{
    public class AnuncioMapping : IEntityTypeConfiguration<Anuncio>
    {
        public void Configure(EntityTypeBuilder<Anuncio> builder)
        {
            builder.ToTable<Anuncio>("tb_AnuncioWebmotors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Marca).IsRequired().HasColumnType("varchar(45)").HasMaxLength(45);
            builder.Property(x => x.Modelo).IsRequired().HasColumnType("varchar(45)").HasMaxLength(45);
            builder.Property(x => x.Versao).IsRequired().HasColumnType("varchar(45)").HasMaxLength(45);
            builder.Property(x => x.Ano).IsRequired();
            builder.Property(x => x.Quilometragem).IsRequired();
            builder.Property(x => x.Observacao).IsRequired().HasColumnType("text");
        }
    }
}
