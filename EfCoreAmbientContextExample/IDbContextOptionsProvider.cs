using Microsoft.EntityFrameworkCore;

namespace EfCoreAmbientContextExample
{
    public interface IDbContextOptionsProvider
    {
        DbContextOptions<T> GetOptions<T>() where T : DbContext;
    }
}