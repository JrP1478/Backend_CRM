using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDireccConfiguration : IEntityTypeConfiguration<Crm_PersDirecc>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDirecc> builder)
        {
            builder.ToTable("Crm_PersDirecc", "dbo");
            builder.HasKey(pd => pd.nId_PersDirecc);

            builder.HasOne(pd => pd.Crm_PersDeudor)
                .WithMany()
                .HasForeignKey(pd => pd.nId_PersDeudor);

            builder.HasOne(pd => pd.Crm_Cliente)
                .WithMany()
                .HasForeignKey(pd => pd.nId_Cliente);

            builder.HasOne(pd => pd.Crm_PersRefUbi)
                .WithMany()
                .HasForeignKey(pd => pd.nId_PersRefUbi);
        }
    }
}