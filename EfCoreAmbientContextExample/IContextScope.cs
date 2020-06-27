using System;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAmbientContextExample
{
    public interface IContextScope : IDisposable
    {
        T GetContext<T>() where T : DbContext; 
        
        void Save();

        void Rollback();
        
        bool IsDisposed { get; }
    }
}