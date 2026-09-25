using GesMgmt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GesMgmt.Infraestructure.Configurations
{
    public class Crm_BotonClienteConfiguration : IEntityTypeConfiguration<Crm_BotonCliente>
    {
        public void Configure(EntityTypeBuilder<Crm_BotonCliente> builder)
        {
            builder.ToTable("Crm_BotonCliente", "dbo");
            builder.HasKey(boton => boton.nId_Boton);

            builder.HasOne(car => car.Crm_Cliente)
                .WithMany()
                .HasForeignKey(car => car.nId_Cliente);

            builder.HasOne(car => car.Crm_Contrato)
                .WithMany()
                .HasForeignKey(car => car.nId_Contrato);
        }
    }
}