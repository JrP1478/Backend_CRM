using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_asigUsuarioConfiguration : IEntityTypeConfiguration<Crm_asigUsuario>
    {
        public void Configure(EntityTypeBuilder<Crm_asigUsuario> builder)
        {
            builder.ToTable("Crm_asigUsuario", "dbo");
            builder.HasKey(cpc => cpc.nid_asignacion);
        }
    }
}