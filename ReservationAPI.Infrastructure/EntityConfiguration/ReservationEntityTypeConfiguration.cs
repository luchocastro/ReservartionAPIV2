using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Infrastructure.Context;
using ReservationAPI.Infrastructure.Context.Model;
namespace ReservationAPI.Infrastructure.EntityConfiguration;
class ReservationEntityTypeConfiguration
    : IEntityTypeConfiguration<WriteReservation>
{
    public void Configure(EntityTypeBuilder<WriteReservation> ReservationConfiguration)
    {
        //ReservationConfiguration
        //    .HasIndex("ClientName", "Date")
        //    .IsUnique(true);
        ReservationConfiguration.HasKey(b => b.Id);
        ReservationConfiguration.HasIndex("Id")
            .IsUnique(true)
            ;
        ReservationConfiguration.Property(b => b.Id)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        ReservationConfiguration.Property(b => b.Hour)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        ReservationConfiguration.Property(b => b.Date)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        ReservationConfiguration.Property(b => b.ClientName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            ;
        ReservationConfiguration
            .Property(b => b.Service)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Service")
            .IsRequired();
        ReservationConfiguration.ToTable("Reservation");
    }
}