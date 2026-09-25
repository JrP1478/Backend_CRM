using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DetallePersTelefConfiguration : IEntityTypeConfiguration<Crm_DetallePersTelef>
    {
        public void Configure(EntityTypeBuilder<Crm_DetallePersTelef> builder)
        {
            builder.ToTable("Crm_DetallePersTelef", "dbo");
            builder.HasKey(car => car.nId_DetallePersTelef);

            builder.Property(car => car.nId_Fuente).HasColumnName("nfuenteBusDet");

            builder.HasOne(car => car.Crm_Cliente)
                .WithMany()
                .HasForeignKey(car => car.nId_Cliente);

            builder.HasOne(car => car.Crm_PersTelef)
                .WithMany()
                .HasForeignKey(car => car.nId_PersTelef);
        }
    }
}