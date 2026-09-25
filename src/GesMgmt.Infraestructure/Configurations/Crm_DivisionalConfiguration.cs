using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_DivisionalConfiguration : IEntityTypeConfiguration<Crm_Divisional>
    {
        public void Configure(EntityTypeBuilder<Crm_Divisional> builder)
        {
            builder.ToTable("Crm_Divisional", "dbo");
            builder.HasKey(dc => dc.nid_division);
        }
    }
}