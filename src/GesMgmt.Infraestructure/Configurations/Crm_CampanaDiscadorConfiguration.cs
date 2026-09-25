using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_CampanaDiscadorConfiguration : IEntityTypeConfiguration<Crm_CampanaDiscador>
    {
        public void Configure(EntityTypeBuilder<Crm_CampanaDiscador> builder)
        {
            builder.ToTable("Crm_CampanaDiscador", "dbo");
            builder.HasKey(car => car.id);
        }
    }
}