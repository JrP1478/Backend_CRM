using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_EstadoEnvioEmailGenConfiguration : IEntityTypeConfiguration<Crm_EstadoEnvioEmailGen>
    {
        public void Configure(EntityTypeBuilder<Crm_EstadoEnvioEmailGen> builder)
        {
            builder.ToTable("Crm_EstadoEnvioEmailGen", "dbo");
            builder.HasKey(car => car.nId_EstadoEnvioEmailGen);
        }
    }
}