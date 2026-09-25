using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDeudorGestionHrsConfiguration : IEntityTypeConfiguration<Crm_PersDeudorGestionHrs>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDeudorGestionHrs> builder)
        {
            builder.ToTable("Crm_PersDeudorGestionHrs", "dbo");
            builder.HasKey(car => car.nId_PersDeudorGestionHrs);
        }
    }
}