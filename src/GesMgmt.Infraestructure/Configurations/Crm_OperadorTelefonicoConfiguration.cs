using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_OperadorTelefonicoConfiguration : IEntityTypeConfiguration<Crm_OperadorTelefonico>
    {
        public void Configure(EntityTypeBuilder<Crm_OperadorTelefonico> builder)
        {
            builder.ToTable("Crm_OperadorTelefonico", "dbo");
            builder.HasKey(car => car.nId_OperadorTelefonico);
        }
    }
}