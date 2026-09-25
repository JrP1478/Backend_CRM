using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_UbigeoConfiguration : IEntityTypeConfiguration<Crm_Ubigeo>
    {
        public void Configure(EntityTypeBuilder<Crm_Ubigeo> builder)
        {
            builder.ToTable("Crm_Ubigeo", "dbo");
            builder.HasKey(car => car.nId_Ubigeo);
        }
    }
}