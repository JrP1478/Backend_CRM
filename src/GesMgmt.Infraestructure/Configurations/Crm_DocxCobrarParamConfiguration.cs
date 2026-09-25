using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarParamConfiguration : IEntityTypeConfiguration<Crm_DocxCobrarParam>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrarParam> builder)
        {
            builder.ToTable("Crm_DocxCobrarParam", "dbo");
            builder.HasKey(dcp => dcp.nId_DocxCobrarParam);

            builder.HasOne(dc => dc.Crm_Cartera)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cartera);

            builder.HasOne(dc => dc.Crm_DocxCobrar)
                .WithMany()
                .HasForeignKey(dc => dc.nId_DocxCobrar);

        }
    }
}