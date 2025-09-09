using System.Data;

namespace Zamp.Server.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}