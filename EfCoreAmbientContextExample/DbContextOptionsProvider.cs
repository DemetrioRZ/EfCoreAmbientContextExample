using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfCoreAmbientContextExample
{
    public class DbContextOptionsProvider : IDbContextOptionsProvider
    {
        private readonly IConfiguration _configuration;

        private readonly Dictionary<string, string> _connectionStrings;
        
        public DbContextOptionsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStrings = new Dictionary<string, string>();
        }
        
        public DbContextOptions<T> GetOptions<T>() where T : DbContext
        {
            var connectionStringName = typeof(T).Name.Replace("Context", string.Empty);

            if (_connectionStrings.ContainsKey(connectionStringName))
            {
                return BuildOptions<T>(_connectionStrings[connectionStringName]);
            }
            
            var connectionString = _configuration.GetConnectionString(connectionStringName);
            _connectionStrings[connectionStringName] = connectionString;

            return BuildOptions<T>(connectionString);
        }

        private DbContextOptions<T> BuildOptions<T>(string connectionString) where T : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }
    }
}