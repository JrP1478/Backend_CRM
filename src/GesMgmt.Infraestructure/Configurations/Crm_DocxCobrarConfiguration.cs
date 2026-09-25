using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarConfiguration : IEntityTypeConfiguration<Crm_DocxCobrar>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrar> builder)
        {
            builder.ToTable("Crm_DocxCobrar", "dbo");
            builder.HasKey(dc => dc.nId_DocxCobrar);

            builder.Property(dc => dc.nId_Usuario).HasColumnName("nid_OpeTelef");

            builder.HasOne(dc => dc.Crm_Cliente)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cliente);

            builder.HasOne(dc => dc.Crm_Cartera)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Cartera);

            builder.HasOne(dc => dc.Crm_PersDeudor)
                .WithMany()
                .HasForeignKey(dc => dc.nId_PersDeudor);

            builder.HasOne(dc => dc.Crm_Moneda)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Moneda);

            builder.HasOne(dc => dc.Crm_Usuario)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Usuario);

            //builder.Property(dc => dc.nImpTotal)
            //    .HasPrecision(18, 2);

            //builder.Property(dc => dc.nSaldoTotal)
            //    .HasPrecision(18, 2);
        }
    }
}
