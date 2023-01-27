using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace GuildManager.Models.Providers;

public abstract class SqliteBaseRepository
{
    private static string DbFile => $"{Environment.CurrentDirectory}\\GuildManager.sqlite";

    private static string ConnectionString => $"Data Source={DbFile};";

    protected static void CreateDatabase(string query)
    {
        using var connection = new SqliteConnection(ConnectionString);

        connection.Open();
        connection.ExecuteAsync(query);
        connection.Close();
    }

    protected static T? GetItemQuery<T>(string query, object param)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var result = connection.Query(query, param).FirstOrDefault();
        connection.Close();

        return result;
    }

    protected static IEnumerable<T>? GetMultipleQuery<T>(string query, object param)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var queryResult = connection.QueryMultiple(query, param);
        var result = queryResult.Read<T>();
        connection.Close();

        return result;
    }

    protected static int ExecuteQuery(string query, object param)
    {
        using var connection = new SqliteConnection(ConnectionString);

        connection.Open();
        var result = connection.Execute(query, param);
        connection.Close();

        return result;
    }
}