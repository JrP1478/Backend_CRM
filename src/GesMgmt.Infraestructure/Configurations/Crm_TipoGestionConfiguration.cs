using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_TipoGestionConfiguration : IEntityTypeConfiguration<Crm_TipoGestion>
    {
        public void Configure(EntityTypeBuilder<Crm_TipoGestion> builder)
        {
            builder.ToTable("Crm_TipoGestion", "dbo");
            builder.HasKey(car => car.nId_TipoGestion);
        }
    }
}