using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_UsuarioConfiguration : IEntityTypeConfiguration<Crm_Usuario>
    {
        public void Configure(EntityTypeBuilder<Crm_Usuario> builder)
        {
            builder.ToTable("Crm_Usuario", "dbo");
            builder.HasKey(car => car.nId_Usuario);
        }
    }
}