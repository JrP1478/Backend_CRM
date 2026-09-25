using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OpcionConfiguration : IEntityTypeConfiguration<Crm_Opcion>
    {
        public void Configure(EntityTypeBuilder<Crm_Opcion> builder)
        {
            builder.ToTable("Crm_Opcion", "dbo");
            builder.HasKey(o => o.nId_Opcion);
        }
    }
}