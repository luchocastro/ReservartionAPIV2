﻿
namespace ReservationAPI.Domain.Common 
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}

