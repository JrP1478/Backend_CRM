using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ContFormularioRptcConfiguration : IEntityTypeConfiguration<Crm_ContFormularioRptc>
    {
        public void Configure(EntityTypeBuilder<Crm_ContFormularioRptc> builder)
        {
            builder.ToTable("Crm_ContFormularioRptc", "dbo");
            builder.HasKey(car => car.nId_ContFormRptc);

            builder.HasOne(car => car.Crm_Contrato)
                .WithMany()
                .HasForeignKey(car => car.nId_Contrato);

            builder.HasOne(car => car.Crm_ContFormTipoCrud)
                .WithMany()
                .HasForeignKey(car => car.nTipoFormCrud);
        }
    }
}