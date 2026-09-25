using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OpeCodCliOutConfiguration : IEntityTypeConfiguration<Crm_OpeCodCliOut>
    {
        public void Configure(EntityTypeBuilder<Crm_OpeCodCliOut> builder)
        {
            builder.ToTable("Crm_OpeCodCliOut", "dbo");
            builder.HasKey(ocl => ocl.nId_OpeCodCliOut);

            builder.HasOne(dc => dc.Crm_Cliente)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cliente);
        }
    }
}