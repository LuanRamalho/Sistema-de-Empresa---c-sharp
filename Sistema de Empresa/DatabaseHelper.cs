using Microsoft.Data.Sqlite;
using System.IO;

public static class DatabaseHelper
{
    public const string DatabaseFile = "empresa.db";
    // A Connection String muda levemente para Microsoft.Data.Sqlite
    private const string ConnectionString = $"Data Source={DatabaseFile}";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    public static void InitializeDatabase()
    {
        // Microsoft.Data.Sqlite cria o arquivo sozinho ao dar conn.Open()
        using (var conn = GetConnection())
        {
            conn.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS funcionarios (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                nome TEXT NOT NULL,
                                funcao TEXT NOT NULL,
                                salario REAL NOT NULL,
                                data_admissao TEXT NOT NULL,
                                data_demissao TEXT
                            );";
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}