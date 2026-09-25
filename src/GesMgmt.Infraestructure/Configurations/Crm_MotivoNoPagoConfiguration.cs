using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_MotivoNoPagoConfiguration : IEntityTypeConfiguration<Crm_MotivoNoPago>
    {
        public void Configure(EntityTypeBuilder<Crm_MotivoNoPago> builder)
        {
            builder.ToTable("Crm_MotivoNoPago", "dbo");
            builder.HasKey(car => car.nId_MotivoNoPago);
        }
    }
}