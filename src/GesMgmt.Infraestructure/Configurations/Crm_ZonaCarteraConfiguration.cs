using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ZonaCarteraConfiguration : IEntityTypeConfiguration<Crm_ZonaCartera>
    {
        public void Configure(EntityTypeBuilder<Crm_ZonaCartera> builder)
        {
            builder.ToTable("Crm_ZonaCartera", "dbo");
            builder.HasKey(car => car.zona);

            builder.Property(dc => dc.nId_Usuario).HasColumnName("nid_usuarioAsistente");

            builder.HasOne(dc => dc.Crm_Divisional)
                .WithMany()
                .HasForeignKey(dc => dc.nid_division);

            //builder.HasOne(dc => dc.Crm_Cliente)
            //    .WithMany()
            //    .HasForeignKey(dc => dc.nid_cliente);

            builder.HasOne(dc => dc.Crm_OficinaCrm)
                .WithMany()
                .HasForeignKey(dc => dc.nid_OficinaCrm);

            builder.HasOne(dc => dc.Crm_Usuario)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Usuario);

            builder.HasOne(dc => dc.Crm_SubZonaGeneral)
                .WithMany()
                .HasForeignKey(dc => dc.nId_SubZonaGen);
        }
    }
}