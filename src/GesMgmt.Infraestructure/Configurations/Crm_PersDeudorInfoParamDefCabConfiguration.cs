using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDeudorInfoParamDefCabConfiguration : IEntityTypeConfiguration<Crm_PersDeudorInfoParamDefCab>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDeudorInfoParamDefCab> builder)
        {
            builder.HasNoKey();
            builder.ToTable("Crm_PersDeudorInfoParamDefCab", "dbo");
        }
    }
}