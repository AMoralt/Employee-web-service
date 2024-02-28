using System.ComponentModel;
using System.Data;
using Dapper;

namespace Infrastructure.Extensions;

public static class DapperExtensions
{
    public static bool UpdateFields<T>(this IDbConnection connection, object param, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var names = new List<string>();

        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(param))
        {
            var value = property.GetValue(param);
            
            if (value != null)
                names.Add(property.Name);
        }

        if (names.Count > 0)
        {
            var sql = string.Format("UPDATE {1} SET {0} WHERE Id=@Id;", string.Join(",", names.Select(t => { t = t + "=@" + t; return t; })), typeof(T).Name);
            var affectedRows = connection.Execute(sql, param, transaction, commandTimeout, commandType);
            
            return affectedRows > 0;
        }
        return false;
    }

    public static bool UpdateFields<T>(this IDbConnection connection, object fields, CommandDefinition commandDefinition)
    {
        return UpdateFields<T>(connection, fields, commandDefinition.Transaction, commandDefinition.CommandTimeout, commandDefinition.CommandType);
    }
}