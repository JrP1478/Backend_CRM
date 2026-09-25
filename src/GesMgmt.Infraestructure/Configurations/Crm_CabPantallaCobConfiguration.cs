using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_CabPantallaCobConfiguration : IEntityTypeConfiguration<Crm_CabPantallaCob>
    {
        public void Configure(EntityTypeBuilder<Crm_CabPantallaCob> builder)
        {
            builder.ToTable("Crm_CabPantallaCob", "dbo");
            builder.HasKey(cpc => cpc.nId_CabPantalla);
        }
    }
}