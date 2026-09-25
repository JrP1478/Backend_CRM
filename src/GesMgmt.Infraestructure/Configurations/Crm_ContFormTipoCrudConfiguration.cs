using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ContFormTipoCrudConfiguration : IEntityTypeConfiguration<Crm_ContFormTipoCrud>
    {
        public void Configure(EntityTypeBuilder<Crm_ContFormTipoCrud> builder)
        {
            builder.ToTable("Crm_ContFormTipoCrud", "dbo");
            builder.HasKey(c => c.nTipoFormCrud);
        }
    }
}