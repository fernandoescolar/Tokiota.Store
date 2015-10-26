namespace Tokiota.Store.Domain
{
    using System.Collections.Generic;

    public interface ICallResult
    {
        bool Succeeded { get; }
        IEnumerable<string> Errors { get; }
    }
}