﻿using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIvoire.Infrastructure.Persistence.Configurations;

public class CommuneEntityTypeConfiguration : IEntityTypeConfiguration<Commune>
{
    public void Configure(EntityTypeBuilder<Commune> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nom).IsRequired();
        builder.HasOne(r => r.Departement)
               .WithMany(d => d.Communes)
               .HasForeignKey(r => r.DepartementId);
    }
}
