using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersRefUbiConfiguration : IEntityTypeConfiguration<Crm_PersRefUbi>
    {
        public void Configure(EntityTypeBuilder<Crm_PersRefUbi> builder)
        {
            builder.ToTable("Crm_PersRefUbi", "dbo");
            builder.HasKey(car => car.nId_PersRefUbi);
        }
    }
}