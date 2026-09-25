using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PasswordHisConfiguration : IEntityTypeConfiguration<Crm_PasswordHis>
    {
        public void Configure(EntityTypeBuilder<Crm_PasswordHis> builder)
        {
            builder.ToTable("Crm_PasswordHis", "dbo");
            builder.HasKey(pw => pw.nId_PasswordHis);

            builder.HasOne(pw => pw.Crm_Usuario)
            .WithMany()
            .HasForeignKey(pw => pw.nId_Usuario);
        }
    }
}