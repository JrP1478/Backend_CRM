using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_MonedaConfiguration : IEntityTypeConfiguration<Crm_Moneda>
    {
        public void Configure(EntityTypeBuilder<Crm_Moneda> builder)
        {
            builder.ToTable("Crm_Moneda", "dbo");
            builder.HasKey(mon => mon.nId_Moneda);
        }
    }
}