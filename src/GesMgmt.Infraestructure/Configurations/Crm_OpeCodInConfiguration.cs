using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OpeCodInConfiguration : IEntityTypeConfiguration<Crm_OpeCodIn>
    {
        public void Configure(EntityTypeBuilder<Crm_OpeCodIn> builder)
        {
            builder.ToTable("Crm_OpeCodIn", "dbo");
            builder.HasKey(car => car.nId_OpeCodIn);

            builder.HasOne(pd => pd.Crm_OpeTipo)
                .WithMany()
                .HasForeignKey(pd => pd.nId_OpeTipo);
        }
    }
}