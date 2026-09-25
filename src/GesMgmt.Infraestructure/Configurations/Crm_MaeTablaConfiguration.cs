using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_MaeTablaConfiguration : IEntityTypeConfiguration<Crm_MaeTabla>
    {
        public void Configure(EntityTypeBuilder<Crm_MaeTabla> builder)
        {
            builder.ToTable("Crm_MaeTabla", "dbo");
            builder.HasKey(dc => dc.nid_tabla);
        }
    }
}