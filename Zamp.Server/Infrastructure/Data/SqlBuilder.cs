using Dapper;
using Microsoft.AspNetCore.Http;
using Zamp.Shared.Extensions;
using Zamp.Shared.Models.Criteria;

namespace Zamp.Server.Infrastructure.Data;

public class SqlBuilder
{
    private const string DefaultOrderBy = "Id ASC";
    private readonly StringBuilder _where = new("WHERE 1=1");
    private readonly List<string> _joins = [];
    private string _select = "*";
    private string _from = "";
    private string _orderBy = DefaultOrderBy;
    private string _pagination = "";
    private readonly Dictionary<string, object> _parameters = new();

    public SqlBuilder Clear()
    {
        _where.Clear();
        _where.Append("WHERE 1=1");
        _joins.Clear();
        _select = "*";
        _from = "";
        _orderBy = DefaultOrderBy;
        _pagination = "";
        _parameters.Clear();
        return this;
    }

    public SqlBuilder Select(string select)
    {
        _select = select;
        return this;
    }

    public SqlBuilder From(string from)
    {
        _from = from;
        return this;
    }

    public SqlBuilder Join(string join)
    {
        _joins.Add(join);
        return this;
    }

    public SqlBuilder AddEqual(string propertyName, object? value)
    {
        if (HasValue(value))
        {
            _where.Append($"\nAND {propertyName.DatabaseColumnName()} = @{propertyName}");
            _parameters[propertyName] = value!;
        }

        return this;
    }

    public SqlBuilder AddLike(string propertyName, string? value, bool caseInsensitive = true)
    {
        if (HasValue(value))
        {
            var op = caseInsensitive ? "ILIKE" : "LIKE";
            _where.Append($"\nAND {propertyName.DatabaseColumnName()} {op} @{propertyName}");
            _parameters[propertyName] = $"%{value}%";
        }

        return this;
    }

    public SqlBuilder AddRange(string propertyName, object? min, object? max)
    {
        if (min is not null)
        {
            var minParam = $"Min{propertyName}";
            _where.Append($"\nAND {propertyName.DatabaseColumnName()} >= @{minParam}");
            _parameters[minParam] = min;
        }

        if (max is not null)
        {
            var maxParam = $"Max{propertyName}";
            _where.Append($"\nAND {propertyName.DatabaseColumnName()} <= @{maxParam}");
            _parameters[maxParam] = max;
        }

        return this;
    }

    public SqlBuilder OrderBy(string propertyName, bool descending = false)
    {
        _orderBy = $"ORDER BY {propertyName.DatabaseColumnName()} {(descending ? "DESC" : "ASC")}";
        return this;
    }

    public SqlBuilder OrderBy(GridCriteriaModel criteria)
    {
        _orderBy = criteria.GridSorting.OrderByClause();
        return this;
    }

    public SqlBuilder Paginate(GridCriteriaModel criteria)
    {
        // Postgres-style pagination; adjust as needed for your DB
        if (criteria.PageSize <= 0 || criteria.DisablePagination) return this;
        
        _pagination = "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        _parameters["Offset"] = criteria.Offset;
        _parameters["PageSize"] = criteria.PageSize + 1; // always try to grab an extra row (which will be truncated) so we know if there are more rows
        return this;
    }
    
    public SqlBuilder WithOrderByAndPagination(GridCriteriaModel criteria)
    {
        OrderBy(criteria);
        Paginate(criteria);
        return this;
    }

    public string RowsSql
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_from))
                throw new InvalidOperationException("FROM clause is required.");

            var joins = $"{(_joins is [] ? "" : "\n")}{string.Join("\n", _joins)}";
            var sql = $"""
                       SELECT {_select}
                       FROM {_from}{joins}
                       {_where}
                       {_orderBy}
                       {_pagination}
                       """.Trim();
            return sql;
        }
    }

    public string CountSql
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_from))
                throw new InvalidOperationException("FROM clause is required.");

            var joins = $"{(_joins is [] ? "" : "\n")}{string.Join("\n", _joins)}";
            var sql = $"""
                       SELECT COUNT(*)
                       FROM {_from}{joins}
                       {_where}
                       """.Trim();
            return sql;
        }
    }

    public DynamicParameters Parameters => new(_parameters);

    private static bool HasValue(object? value)
    {
        if (value is string s)
            return !string.IsNullOrWhiteSpace(s);
        return value != null;
    }
}