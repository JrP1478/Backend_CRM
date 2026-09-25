using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersEmailConfiguration : IEntityTypeConfiguration<Crm_PersEmail>
    {
        public void Configure(EntityTypeBuilder<Crm_PersEmail> builder)
        {
            builder.ToTable("Crm_PersEmail", "dbo");
            builder.HasKey(car => car.nId_PersEmail);

            builder.HasOne(car => car.Crm_PersDeudor)
                .WithMany()
                .HasForeignKey(car => car.nId_PersDeudor);
        }
    }
}