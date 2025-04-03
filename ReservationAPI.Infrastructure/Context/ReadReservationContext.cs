using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Domain.Common;
using ReservationAPI.Infrastructure.Context.Model;
using ReservationAPI.Infrastructure.EntityConfiguration;
using ReservationAPI.Infrastructure.Extentions;
using ReservationAPI.Infrastructure.Extentions.Extentions.Infrastructure;
using System;
using System.Data;


namespace ReservationAPI.Infrastructure.Context;

public class ReadReservationContext : DbContext 
{
    public DbSet<ReadReservation> Reservations { get; set; }
    
    public ReadReservationContext(DbContextOptions<ReadReservationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configuration = new ReadReservationEntityTypeConfiguration();
        modelBuilder.ApplyConfiguration<ReadReservation>(configuration);
    }
}