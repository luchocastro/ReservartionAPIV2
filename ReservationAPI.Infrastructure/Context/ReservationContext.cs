using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Domain.Common;
using ReservationAPI.Infrastructure.EntityConfiguration;
using ReservationAPI.Infrastructure.Extentions;
using ReservationAPI.Infrastructure.Extentions.Extentions.Infrastructure;
using System;
using System.Data;
using static ReservationAPI.Infrastructure.Context.ReservationesignFactory;


namespace ReservationAPI.Infrastructure.Context;

public class ReservationContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "ordering";
    public DbSet<Reservation> Reservations { get; init; }
    public DbSet<EventLog> EventLogs { get; init; }

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public ReservationContext(DbContextOptions<ReservationContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ReservationEntityTypeConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _mediator.DispatchDomainEventsAsync(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

public class ReservationesignFactory : IDesignTimeDbContextFactory<ReservationContext>
{
    public ReservationContext CreateDbContext(string[] args)
    {

    var optionsBuilder = new DbContextOptionsBuilder<ReservationContext>()
            .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.OrderingDb;Integrated Security=true");

        return new ReservationContext(optionsBuilder.Options, new NoMediator());
    }

}
public class NoMediator : IMediator
{
    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return default(IAsyncEnumerable<TResponse>);
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
    {
        return default(IAsyncEnumerable<object?>);
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        return Task.CompletedTask;
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult<TResponse>(default(TResponse));
    }

    public Task<object> Send(object request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(default(object));
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        throw new NotImplementedException();
    }
}