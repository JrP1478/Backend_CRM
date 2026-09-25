using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OpeCodCliOutEstConfiguration : IEntityTypeConfiguration<Crm_OpeCodCliOutEst>
    {
        public void Configure(EntityTypeBuilder<Crm_OpeCodCliOutEst> builder)
        {
            builder.ToTable("Crm_OpeCodCliOutEst", "dbo");
            builder.HasKey(car => car.nId_OpeCodCliOut);
        }
    }
}