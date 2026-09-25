using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ConfigSistemaConfiguration : IEntityTypeConfiguration<Crm_ConfigSistema>
    {
        public void Configure(EntityTypeBuilder<Crm_ConfigSistema> builder)
        {
            builder.ToTable("Crm_ConfigSistema", "dbo");
            builder.HasKey(cfg => cfg.nCodTabla);
            builder.HasKey(cfg => cfg.cLlave);
        }
    }
}