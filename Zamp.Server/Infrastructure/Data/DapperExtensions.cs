// using System.Data;
// using Dapper;
// using System.Collections.Generic;
// using System.Threading.Tasks;
//
// namespace Amp.Case.Server.Infrastructure.Data;
//
// public static class DapperExtensions
// {
//     /// <summary>
//     /// Executes a query and returns the first row or default, using the connection factory.
//     /// </summary>
//     public static async Task<T?> QueryFirstOrDefaultAsync<T>(this IDbConnectionFactory factory, string sql, object? param = null)
//     {
//         using var db = factory.CreateConnection();
//         return await db.QueryFirstOrDefaultAsync<T>(sql, param);
//     }
//
//     /// <summary>
//     /// Executes a query and returns the results as a List, using the connection factory.
//     /// </summary>
//     public static async Task<List<T>> QueryListAsync<T>(this IDbConnectionFactory factory, string sql, object? param = null)
//     {
//         using var db = factory.CreateConnection();
//         var result = await db.QueryAsync<T>(sql, param);
//         return result.AsList();
//     }
//
//     /// <summary>
//     /// Executes a scalar query and returns the result, or a default value if null, using the connection factory.
//     /// </summary>
//     public static async Task<T> QueryScalarOrDefaultAsync<T>(this IDbConnectionFactory factory, string sql, object? param = null, T defaultValue = default!)
//     {
//         using var db = factory.CreateConnection();
//         var result = await db.ExecuteScalarAsync<T>(sql, param);
//         return result ?? defaultValue;
//     }
//
//     /// <summary>
//     /// Executes an insert and returns the generated key (requires RETURNING clause in PostgreSQL), using the connection factory.
//     /// </summary>
//     public static async Task<TKey> InsertAndReturnKeyAsync<TKey>(this IDbConnectionFactory factory, string sql, object param)
//     {
//         using var db = factory.CreateConnection();
//         return await db.ExecuteScalarAsync<TKey>(sql, param);
//     }
//
//     /// <summary>
//     /// Bulk insert for a list of items, using the connection factory.
//     /// </summary>
//     public static async Task<int> BulkInsertAsync<T>(this IDbConnectionFactory factory, string sql, IEnumerable<T> items)
//     {
//         using var db = factory.CreateConnection();
//         return await db.ExecuteAsync(sql, items);
//     }
// }