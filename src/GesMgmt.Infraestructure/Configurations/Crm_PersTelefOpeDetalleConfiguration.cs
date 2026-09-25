using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersTelefOpeDetalleConfiguration : IEntityTypeConfiguration<Crm_PersTelefOpeDetalle>
    {
        public void Configure(EntityTypeBuilder<Crm_PersTelefOpeDetalle> builder)
        {
            builder.ToTable("Crm_PersTelefOpeDetalle", "dbo");
            builder.HasKey(car => car.nId_PersTelefOpeDet);

            builder.HasOne(car => car.Crm_PersTelef)
                .WithMany()
                .HasForeignKey(car => car.nId_PersTelef);

            builder.HasOne(car => car.Crm_PersTelefOpe)
                .WithMany()
                .HasForeignKey(car => car.nId_PersTelefOpe);
        }
    }
}