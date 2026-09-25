using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ZonaGeneralConfiguration : IEntityTypeConfiguration<Crm_ZonaGeneral>
    {
        public void Configure(EntityTypeBuilder<Crm_ZonaGeneral> builder)
        {
            builder.ToTable("Crm_ZonaGeneral", "dbo");
            builder.HasKey(car => car.nId_ZonaGen);

            builder.Property(dc => dc.nId_Usuario).HasColumnName("nId_Coordinador");

            builder.HasOne(dc => dc.Crm_Usuario)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Usuario);
        }
    }
}