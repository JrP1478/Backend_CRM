using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_ContratoConfiguration : IEntityTypeConfiguration<Crm_Contrato>
    {
        public void Configure(EntityTypeBuilder<Crm_Contrato> builder)
        {
            builder.ToTable("Crm_Contrato", "dbo");
            builder.HasKey(con => con.nId_Contrato);

            builder.HasOne(car => car.Crm_Cliente)
                .WithMany()
                .HasForeignKey(car => car.nId_Cliente);
        }
    }
}