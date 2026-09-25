using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ClienteConfiguration : IEntityTypeConfiguration<Crm_Cliente>
    {
        public void Configure(EntityTypeBuilder<Crm_Cliente> builder)
        {
            builder.ToTable("Crm_Cliente", "dbo");
            builder.HasKey(cli => cli.nId_Cliente);
        }
    }
}