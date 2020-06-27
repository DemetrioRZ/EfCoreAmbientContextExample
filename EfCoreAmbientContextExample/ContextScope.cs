using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EfCoreAmbientContextExample
{
    public class ContextScope : IContextScope, INestedScope
    {
        private readonly IDbContextOptionsProvider _dbContextOptionsProvider;
        
        private readonly Dictionary<string, DbContext> _contextsDictionary;

        private IDbContextTransaction _scopeDbContextTransaction;
        
        public uint NestedUsingCount { get; set; }

        public ContextScope(IDbContextOptionsProvider dbContextOptionsProvider)
        {
            _dbContextOptionsProvider = dbContextOptionsProvider;
            _contextsDictionary = new Dictionary<string, DbContext>();
        }
        
        public T GetContext<T>() where T : DbContext
        {
            var key = typeof(T).Name;

            if (_contextsDictionary.ContainsKey(key) && (_contextsDictionary[key] is T context))
                return context;

            var options = _dbContextOptionsProvider.GetOptions<T>();
            
            var newContext = (T)Activator.CreateInstance(typeof(T), options );

            _contextsDictionary[key] = newContext;

            if (_scopeDbContextTransaction == null)
                _scopeDbContextTransaction = newContext.Database.BeginTransaction();
            else
                newContext.Database.UseTransaction(_scopeDbContextTransaction.GetDbTransaction());
            
            return newContext;
        }

        public void Save()
        {
            if (NestedUsingCount != 0)
                return;
            
            _scopeDbContextTransaction.Commit();
        }

        public void Rollback()
        {
            if (NestedUsingCount != 0)
                return;
            
            _scopeDbContextTransaction.Rollback();
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (NestedUsingCount != 0)
            {
                NestedUsingCount--;
                return;
            }
            
            _scopeDbContextTransaction?.Dispose();

            foreach (var dbContext in _contextsDictionary.Values)
                dbContext?.Dispose();

            IsDisposed = true;
        }
    }
}