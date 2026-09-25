using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ContFormTipoParamOpeConfiguration : IEntityTypeConfiguration<Crm_ContFormTipoParamOpe>
    {
        public void Configure(EntityTypeBuilder<Crm_ContFormTipoParamOpe> builder)
        {
            builder.ToTable("Crm_ContFormTipoParamOpe", "dbo");
            builder.HasKey(car => car.nTipoFormParamOpe);
        }
    }
}