using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OficinaCrmConfiguration : IEntityTypeConfiguration<Crm_OficinaCrm>
    {
        public void Configure(EntityTypeBuilder<Crm_OficinaCrm> builder)
        {
            builder.ToTable("Crm_OficinaCrm", "dbo");
            builder.HasKey(car => car.nid_OficinaCrm);

            builder.Property(dc => dc.nId_Usuario).HasColumnName("nid_usuarioResponsable");

            builder.HasOne(dc => dc.Crm_Usuario)
                .WithMany()
                .HasForeignKey(dc => dc.nId_Usuario);
        }
    }
}