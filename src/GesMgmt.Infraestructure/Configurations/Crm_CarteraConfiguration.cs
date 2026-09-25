using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_CarteraConfiguration : IEntityTypeConfiguration<Crm_Cartera>
    {
        public void Configure(EntityTypeBuilder<Crm_Cartera> builder)
        {
            builder.ToTable("Crm_Cartera","dbo");
            builder.HasKey(car => car.nId_Cartera);

            builder.HasOne(car => car.Crm_Cliente)
                .WithMany()
                .HasForeignKey(car => car.nId_Cliente);

            builder.HasOne(car => car.Crm_Contrato)
                .WithMany()
                .HasForeignKey(car => car.nId_Contrato);
        }
    }
}