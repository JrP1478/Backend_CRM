using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DocxPagoConfiguration : IEntityTypeConfiguration<Crm_DocxPago>
    {
        public void Configure(EntityTypeBuilder<Crm_DocxPago> builder)
        {
            builder.ToTable("Crm_DocxPago", "dbo");
            builder.HasKey(cpc => cpc.nId_DocxPago);
        }
    }
}