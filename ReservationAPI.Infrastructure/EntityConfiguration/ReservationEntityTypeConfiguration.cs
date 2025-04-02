using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Infrastructure.Context;
namespace ReservationAPI.Infrastructure.EntityConfiguration;
class ReservationEntityTypeConfiguration
    : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> ReservationConfiguration)
    {   
        ReservationConfiguration.HasKey(b => b.Id);
        ReservationConfiguration.HasIndex("Id")
            .IsUnique(true);
        ReservationConfiguration.Property(b => b.Hour);
        ReservationConfiguration.Property(b => b.Date);
        ReservationConfiguration.Property(b => b.ClientName);
        ReservationConfiguration.Property(b => b.Service);
        ReservationConfiguration.ToTable("Reservation");
    }
}