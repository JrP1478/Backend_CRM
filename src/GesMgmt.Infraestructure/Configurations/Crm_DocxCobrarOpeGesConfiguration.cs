using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarOpeGesConfiguration : IEntityTypeConfiguration<Crm_DocxCobrarOpeGes>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrarOpeGes> builder)
        {
            builder.ToTable("Crm_DocxCobrarOpeGes", "dbo");
            builder.HasKey(dco => dco.nId_DocxCobrarOpeGes);

            builder.Property(dco => dco.nId_TipoGestion).HasColumnName("tip_gestion");
            builder.Property(dco => dco.nId_Usuario).HasColumnName("nId_UsuOpe");
            builder.Property(dco => dco.nId_OpeCodCliOut).HasColumnName("nId_OpeCodOut");

            builder.HasOne(dco => dco.Crm_DocxCobrar)
                .WithMany()
                .HasForeignKey(dc => dc.nId_DocxCobrar);

            builder.HasOne(dco => dco.Crm_Usuario)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Usuario);

            builder.HasOne(dco => dco.Crm_TipoGestion)
                .WithMany()
                .HasForeignKey(dc => dc.nId_TipoGestion);

            builder.HasOne(dco => dco.Crm_OpeCodCliOut)
                .WithMany()
                .HasForeignKey(dc => dc.nId_OpeCodCliOut);
        }
    }
}