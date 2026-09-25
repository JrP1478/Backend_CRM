using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDeudorConfiguration : IEntityTypeConfiguration<Crm_PersDeudor>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDeudor> builder)
        {
            builder.ToTable("Crm_PersDeudor", "dbo");
            builder.HasKey(car => car.nId_PersDeudor);
        }
    }
}
