using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_FuenteBusTelConfiguration : IEntityTypeConfiguration<Crm_FuenteBusTel>
    {
        public void Configure(EntityTypeBuilder<Crm_FuenteBusTel> builder)
        {
            builder.ToTable("Crm_FuenteBusTel", "dbo");
            builder.HasKey(cpc => cpc.nId_Fuente);
        }
    }
}