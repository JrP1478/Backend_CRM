using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_SubZonaGeneralConfiguration : IEntityTypeConfiguration<Crm_SubZonaGeneral>
    {
        public void Configure(EntityTypeBuilder<Crm_SubZonaGeneral> builder)
        {
            builder.ToTable("Crm_SubZonaGeneral", "dbo");
            builder.HasKey(pd => pd.nId_SubZonaGen);

            //builder.HasOne(pd => pd.Crm_ZonaGeneral)
            //   .WithMany()
            //   .HasForeignKey(pd => pd.nId_ZonaGen);
        }
    }
}