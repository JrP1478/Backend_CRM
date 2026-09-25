using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_AgendaConfiguration : IEntityTypeConfiguration<Crm_Agenda>
    {
        public void Configure(EntityTypeBuilder<Crm_Agenda> builder)
        {
            builder.ToTable("Crm_Agenda", "dbo");
            builder.HasKey(cpc => cpc.nid_agenda);
        }
    }
}