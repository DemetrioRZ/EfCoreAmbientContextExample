﻿namespace EfCoreAmbientContextExample
{
    public class ContextScopeFactory : IContextScopeFactory
    {
        private readonly IDbContextOptionsProvider _dbContextOptionsProvider;

        private IContextScope _existingContextScope;

        public ContextScopeFactory(IDbContextOptionsProvider dbContextOptionsProvider)
        {
            _dbContextOptionsProvider = dbContextOptionsProvider;
        }

        public IContextScope CreateContextScope()
        {
            if (_existingContextScope == null || _existingContextScope.IsDisposed)
            {
                _existingContextScope = new ContextScope(_dbContextOptionsProvider);
                return _existingContextScope;
            }
            
            var nested = (INestedScope)_existingContextScope;
            nested.NestedUsingCount++;
            return _existingContextScope;
        }
    }
}