using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OpeTipoConfiguration : IEntityTypeConfiguration<Crm_OpeTipo>
    {
        public void Configure(EntityTypeBuilder<Crm_OpeTipo> builder)
        {
            builder.ToTable("Crm_OpeTipo", "dbo");
            builder.HasKey(car => car.nId_OpeTipo);
        }
    }
}