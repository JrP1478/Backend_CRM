using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PerfilConfiguration : IEntityTypeConfiguration<Crm_Perfil>
    {
        public void Configure(EntityTypeBuilder<Crm_Perfil> builder)
        {
            builder.ToTable("Crm_Perfil", "dbo");
            builder.HasKey(dc => dc.nid_perfil);
        }
    }
}