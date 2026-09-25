using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DiscadorConfiguration : IEntityTypeConfiguration<Crm_Discador>
    {
        public void Configure(EntityTypeBuilder<Crm_Discador> builder)
        {
            builder.ToTable("Crm_Discador", "dbo");
            builder.HasKey(disc => disc.nId_Discador);
        }
    }
}