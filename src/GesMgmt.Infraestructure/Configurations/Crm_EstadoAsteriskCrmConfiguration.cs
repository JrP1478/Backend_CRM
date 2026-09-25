using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_EstadoAsteriskCrmConfiguration : IEntityTypeConfiguration<Crm_EstadoAsteriskCrm>
    {
        public void Configure(EntityTypeBuilder<Crm_EstadoAsteriskCrm> builder)
        {
            builder.ToTable("Crm_EstadoAsteriskCrm", "dbo");
            builder.HasKey(cpc => cpc.nId_EstadoAsteriskCrm);
        }
    }
}