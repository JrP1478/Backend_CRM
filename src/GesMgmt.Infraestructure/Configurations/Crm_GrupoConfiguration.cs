using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_GrupoConfiguration : IEntityTypeConfiguration<Crm_Grupo>
    {
        public void Configure(EntityTypeBuilder<Crm_Grupo> builder)
        {
            builder.ToTable("Crm_Grupo", "dbo");
            builder.HasKey(gr => gr.nId_Grupo);

            builder.HasOne(gr => gr.Crm_Cliente)
                .WithMany()
                .HasForeignKey(gr => gr.nid_cliente);
        }
    }
}