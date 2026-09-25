using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_UGrupoConfiguration : IEntityTypeConfiguration<Crm_UGrupo>
    {
        public void Configure(EntityTypeBuilder<Crm_UGrupo> builder)
        {
            builder.ToTable("Crm_UGrupo", "dbo");
            builder.HasKey(car => car.nId_UGrupo);

            builder.HasOne(pd => pd.Crm_Usuario)
                .WithMany()
                .HasForeignKey(pd => pd.nId_Usuario);

            builder.HasOne(pd => pd.Crm_Grupo)
                .WithMany()
                .HasForeignKey(pd => pd.nId_Grupo);
        }
    }
}