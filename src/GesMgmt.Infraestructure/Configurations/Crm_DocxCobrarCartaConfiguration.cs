using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxCobrarCartaConfiguration : IEntityTypeConfiguration<Crm_DocxCobrarCarta>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxCobrarCarta> builder)
        {
            builder.ToTable("Crm_DocxCobrarCarta", "dbo");
            builder.HasKey(dcp => dcp.nId_DocxCobrar);
        }
    }
}