using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersEmailOpeConfiguration : IEntityTypeConfiguration<Crm_PersEmailOpe>
    {
        public void Configure(EntityTypeBuilder<Crm_PersEmailOpe> builder)
        {
            builder.ToTable("Crm_PersEmailOpe", "dbo");
            builder.HasKey(car => car.nId_PersEmailOpe);
        }
    }
}