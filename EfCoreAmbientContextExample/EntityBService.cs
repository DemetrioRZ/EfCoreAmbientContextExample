using System;
using EfCoreAmbientContextExample.Models;

namespace EfCoreAmbientContextExample
{
    public class EntityBService : IEntityBService
    {
        private readonly IContextScopeFactory _contextScopeFactory;

        public EntityBService(IContextScopeFactory contextScopeFactory)
        {
            _contextScopeFactory = contextScopeFactory;
        }

        public void TestB()
        {
            using (var scope = _contextScopeFactory.CreateContextScope())
            {
                var testDbContext = scope.GetContext<TestDbContext>();

                // var entitiesA = testDbContext.Set<EntityA>().ToList();
                
                var entityB = new EntityB { ValueB = $"some value B {DateTime.Now:s}"};

                testDbContext.Set<EntityB>().Add(entityB);
                
                testDbContext.SaveChanges(); 
                
                scope.Save(); 
            }
        }
    }
}