using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_EstadoEnvioEmailErrorConfiguration : IEntityTypeConfiguration<Crm_EstadoEnvioEmailError>
    {
        public void Configure(EntityTypeBuilder<Crm_EstadoEnvioEmailError> builder)
        {
            builder.ToTable("Crm_EstadoEnvioEmailError", "dbo");
            builder.HasKey(car => car.nId_EstadoEnvioEmail);
        }
    }
}