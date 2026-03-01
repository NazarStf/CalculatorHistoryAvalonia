using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class DatabaseService
{
    private string _connectionString = "Data Source=history.db";

    public DatabaseService()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS History (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Expression TEXT NOT NULL,
            Result TEXT NOT NULL
        );
        ";

        command.ExecuteNonQuery();
    }

    public void SaveResult(string expr, string res)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            "INSERT INTO History (Expression, Result) VALUES ($expr, $res)";

        command.Parameters.AddWithValue("$expr", expr);
        command.Parameters.AddWithValue("$res", res);

        command.ExecuteNonQuery();
    }

    public List<CalculationRecord> GetHistory()
    {
        var list = new List<CalculationRecord>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            "SELECT Id, Expression, Result FROM History";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new CalculationRecord
            {
                Id = reader.GetInt32(0),
                Expression = reader.GetString(1),
                Result = reader.GetString(2)
            });
        }

        return list;
    }
}