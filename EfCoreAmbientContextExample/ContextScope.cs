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

        // remove _scopeDbContextTransaction to use TransactionScope
        private IDbContextTransaction _scopeDbContextTransaction;
        
        // TransactionScope can be used instead of explicit IDbContextTransaction creation,
        // but still no success with distributed transactions on ef core
        // private TransactionScope _transactionScope;
        
        public uint NestedUsingCount { get; set; }

        public ContextScope(IDbContextOptionsProvider dbContextOptionsProvider)
        {
            _dbContextOptionsProvider = dbContextOptionsProvider;
            _contextsDictionary = new Dictionary<string, DbContext>();
        }
        
        public T GetContext<T>() where T : DbContext
        {
            // uncomment this to use TransactionScope
            // if (_transactionScope == null)
            //     _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var key = typeof(T).Name;

            if (_contextsDictionary.ContainsKey(key) && (_contextsDictionary[key] is T context))
                return context;

            var options = _dbContextOptionsProvider.GetOptions<T>();
            
            var newContext = (T)Activator.CreateInstance(typeof(T), options );

            _contextsDictionary[key] = newContext;

            // remove if else to use TransactionScope
            if (_scopeDbContextTransaction == null)
                _scopeDbContextTransaction = newContext.Database.BeginTransaction();
            else
                newContext.Database.UseTransaction(_scopeDbContextTransaction.GetDbTransaction());
            // remove if else to use TransactionScope
            
            return newContext;
        }

        public void Save()
        {
            if (NestedUsingCount != 0)
                return;
            
            // remove _scopeDbContextTransaction.Commit() to use TransactionScope
            _scopeDbContextTransaction.Commit();
            
            // uncomment this to use TransactionScope
            //_transactionScope.Complete();
        }

        public void Rollback()
        {
            if (NestedUsingCount != 0)
                return;
            
            // remove _scopeDbContextTransaction.Rollback() to use TransactionScope
            _scopeDbContextTransaction.Rollback();
            
            // uncomment this to use TransactionScope
            // need to be tested
            // foreach (var dbContext in _contextsDictionary.Values)
            //     dbContext?.Database.RollbackTransaction();
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (NestedUsingCount != 0)
            {
                NestedUsingCount--;
                return;
            }
            
            // remove _scopeDbContextTransaction?.Dispose() to use TransactionScope
            _scopeDbContextTransaction?.Dispose();
            
            // uncomment this to use TransactionScope
            //_transactionScope?.Dispose();

            foreach (var dbContext in _contextsDictionary.Values)
                dbContext?.Dispose();

            IsDisposed = true;
        }
    }
}