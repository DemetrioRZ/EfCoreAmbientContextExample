using EfCoreAmbientContextExample.Mapping;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAmbientContextExample
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityAMap());
            modelBuilder.ApplyConfiguration(new EntityBMap());
        }
    }
}