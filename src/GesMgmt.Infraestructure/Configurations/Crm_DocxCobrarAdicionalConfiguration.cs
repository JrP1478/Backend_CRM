using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarAdicionalConfiguration : IEntityTypeConfiguration<Crm_DocxCobrarAdicional>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrarAdicional> builder)
        {
            builder.ToTable("Crm_DocxCobrarAdicional", "dbo");
            builder.HasKey(dcp => dcp.nId_DocxCobrarAd);

            builder.HasOne(dc => dc.Crm_Cliente)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cliente);

            builder.HasOne(dc => dc.Crm_Cartera)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cartera);

            builder.HasOne(dc => dc.Crm_DocxCobrar)
                .WithMany()
                .HasForeignKey(dc => dc.nId_DocxCobrar);

            builder.HasOne(dc => dc.Crm_PersDeudor)
                .WithMany()
                .HasForeignKey(dc => dc.nId_PersDeudor);
        }
    }
}