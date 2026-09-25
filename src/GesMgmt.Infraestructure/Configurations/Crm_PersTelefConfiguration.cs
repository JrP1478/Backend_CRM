using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersTelefConfiguration : IEntityTypeConfiguration<Crm_PersTelef>
    {
        public void Configure(EntityTypeBuilder<Crm_PersTelef> builder)
        {
            builder.ToTable("Crm_PersTelef", "dbo");
            builder.HasKey(tel => tel.nId_PersTelef);

            builder.Property(tel => tel.baseTelef).HasColumnName("base");
            builder.Property(tel => tel.nId_Fuente).HasColumnName("nfuenteBus");

            builder.HasOne(tel => tel.Crm_PersDeudor)
            .WithMany()
            .HasForeignKey(tel => tel.nId_PersDeudor);

        }
    }
}