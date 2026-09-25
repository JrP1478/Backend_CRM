using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDeudorParamConfiguration : IEntityTypeConfiguration<Crm_PersDeudorParam>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDeudorParam> builder)
        {
            builder.ToTable("Crm_PersDeudorParam", "dbo");
            builder.HasKey(car => car.nId_PersDeudorParam);

            builder.HasOne(car => car.Crm_Cartera)
                .WithMany()
                .HasForeignKey(car => car.nId_Cartera);

            builder.HasOne(car => car.Crm_PersDeudor)
                .WithMany()
                .HasForeignKey(car => car.nId_PersDeudor);
        }
    }
}