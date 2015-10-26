namespace Tokiota.Store.Domain
{
    using System;

    public abstract class AggregateRoot : Entity<Guid>, IAggregateRoot
    {
    }
}
