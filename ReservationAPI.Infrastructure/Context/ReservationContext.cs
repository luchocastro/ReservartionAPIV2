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


namespace ReservationAPI.Infrastructure.Context;

public class WriteReservationContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "ordering";
    public DbSet<Reservation> Reservations { get; init; }
 
    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public WriteReservationContext(DbContextOptions<WriteReservationContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public WriteReservationContext(DbContextOptions<WriteReservationContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configuration = new ReservationEntityTypeConfiguration();
        modelBuilder.ApplyConfiguration<Reservation>(configuration);

    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        
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

//public class NoMediator : IMediator
//{
//    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
//    {
//        return default(IAsyncEnumerable<TResponse>);
//    }

//    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
//    {
//        return default(IAsyncEnumerable<object?>);
//    }

//    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
//    {
//        return Task.CompletedTask;
//    }

//    public Task Publish(object notification, CancellationToken cancellationToken = default)
//    {
//        return Task.CompletedTask;
//    }

//    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
//    {
//        return await Task.FromResult<TResponse>(default(TResponse));
//    }

//    public Task<object> Send(object request, CancellationToken cancellationToken = default)
//    {
//        return Task.FromResult(default(object));
//    }

//    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
//    {
//        throw new NotImplementedException();
//    }
//}