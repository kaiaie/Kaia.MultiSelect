using System;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
