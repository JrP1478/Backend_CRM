using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_UsuarioGrupoOpcionConfiguration : IEntityTypeConfiguration<Crm_UsuarioGrupoOpcion>
    {
        public void Configure(EntityTypeBuilder<Crm_UsuarioGrupoOpcion> builder)
        {
            builder.ToTable("Crm_UsuarioGrupoOpcion", "dbo");
            builder.HasKey(ugo => ugo.nId_UsuarioGrupoOpcion);

            builder.HasOne(ugo => ugo.Crm_Usuario)
                .WithMany()
                .HasForeignKey(ugo => ugo.nId_Usuario);

            builder.HasOne(ugo => ugo.Crm_Grupo)
                .WithMany()
                .HasForeignKey(ugo => ugo.nId_Grupo);

            builder.HasOne(ugo => ugo.Crm_Opcion)
                .WithMany()
                .HasForeignKey(ugo => ugo.nId_Opcion);
        }
    }
}