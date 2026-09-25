using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersTelefOpeConfiguration : IEntityTypeConfiguration<Crm_PersTelefOpe>
    {
        public void Configure(EntityTypeBuilder<Crm_PersTelefOpe> builder)
        {
            builder.ToTable("Crm_PersTelefOpe", "dbo");
            builder.HasKey(car => car.nId_PersTelefOpe);
        }
    }
}