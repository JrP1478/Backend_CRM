using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_PersDeudorInfoParamConfiguration : IEntityTypeConfiguration<Crm_PersDeudorInfoParam>
    {
        public void Configure(EntityTypeBuilder<Crm_PersDeudorInfoParam> builder)
        {
            builder.ToTable("Crm_PersDeudorInfoParam", "dbo");
            builder.HasKey(car => car.nId_PersDeudor);
        }
    }
}