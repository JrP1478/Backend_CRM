using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_TablaCampoGeneralConfiguration : IEntityTypeConfiguration<Crm_TablaCampoGeneral>
    {
        public void Configure(EntityTypeBuilder<Crm_TablaCampoGeneral> builder)
        {
            builder.ToTable("Crm_TablaCampoGeneral", "dbo");
            builder.HasKey(car => car.id_cab);
        }
    }
}