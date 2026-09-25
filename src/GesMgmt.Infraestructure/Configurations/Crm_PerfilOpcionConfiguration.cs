using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PerfilOpcionConfiguration : IEntityTypeConfiguration<Crm_PerfilOpcion>
    {
        public void Configure(EntityTypeBuilder<Crm_PerfilOpcion> builder)
        {
            builder.ToTable("Crm_PerfilOpcion", "dbo");
            builder.HasKey(dc => dc.nId_PerfilOpcion);

            builder.HasOne(po => po.Crm_Perfil)
                   .WithMany()
                   .HasForeignKey(po => po.nId_Perfil);

            builder.HasOne(po => po.Crm_Opcion)
                   .WithMany()
                   .HasForeignKey(po => po.nId_Opcion);
        }
    }
}