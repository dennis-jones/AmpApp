using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Zamp.Server.Infrastructure.Data
{
    public class NpgsqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
    {
        private readonly string _connectionString = configuration.GetConnectionString("AppDb") ?? "";

        public IDbConnection Create() => new NpgsqlConnection(_connectionString);
    }
}