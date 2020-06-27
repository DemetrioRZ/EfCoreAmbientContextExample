using System;
using EfCoreAmbientContextExample.Models;

namespace EfCoreAmbientContextExample
{
    public class EntityAService : IEntityAService
    {
        private readonly IContextScopeFactory _contextScopeFactory;

        private readonly IEntityBService _entityBService;

        public EntityAService(
            IContextScopeFactory contextScopeFactory, 
            IEntityBService entityBService)
        {
            _contextScopeFactory = contextScopeFactory;
            _entityBService = entityBService;
        }

        public void TestA()
        {
            using (var scope = _contextScopeFactory.CreateContextScope())
            {
                var testDbContext = scope.GetContext<TestDbContext>();

                var entityA = new EntityA { ValueA = $"some value A {DateTime.Now:s}"};

                testDbContext.Set<EntityA>().Add(entityA);
                
                testDbContext.SaveChanges(); 
                
                _entityBService.TestB();
                
                scope.Save(); 
            }
        }
    }
}