using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarOpeResultConfiguration : IEntityTypeConfiguration<Crm_DocxCobrarOpeResult>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrarOpeResult> builder)
        {
            builder.ToTable("Crm_DocxCobrarOpeResult", "dbo");
            builder.HasKey(doc => doc.nId_DocxCobrarOpeResult);

            builder.HasOne(car => car.Crm_DocxCobrar)
                .WithMany()
                .HasForeignKey(car => car.nId_DocxCobrar);
        }
    }
}