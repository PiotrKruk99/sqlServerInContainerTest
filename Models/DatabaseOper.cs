using System.Data.SqlClient;
using static System.Console;
using Dapper;
using System.Data;

namespace Models;

public static class DatabaseOper
{
    private static SqlConnectionStringBuilder ConnStr { get; }
    public static SqlConnection Conn { get; }

    static DatabaseOper()
    {
        ConnStr = new SqlConnectionStringBuilder();
        ConnStr.ConnectionString = "Server=localhost;User Id=sa;" +
                "Password=MyVeryStrongPassword1";

        Conn = new SqlConnection(ConnStr.ToString());
    }

    public static IEnumerable<DatabaseDetails> GetDatabasesList()
    {
        string query = """
                        SELECT name, database_id, create_date 
                        FROM sys.databases;
                        """;
        IEnumerable<DatabaseDetails> result = Conn.Query<DatabaseDetails>(query);

        WriteLine("All databases list:");

        foreach (var row in result)
            WriteLine($"   {row.database_id} - {row.name} : {row.create_date.ToString("dd-MM-yyyy")}");

        return result;
    }

    public static void SelectDatabase(IEnumerable<DatabaseDetails> baseList)
    {
        Write("Select database to work with: ");
        bool selectionOK;
        int selectedId;

        do
        {
            selectionOK = true;
            try
            {
                Write("select option: ");
                selectedId = Convert.ToInt32(ReadLine());

                if (selectedId != 0)
                {
                    DatabaseDetails? dataBase = baseList.FirstOrDefault(x => x.database_id == selectedId);

                    if (dataBase is null)
                    {
                        WriteLine("No base with typed ID. Try again");
                        selectionOK = false;
                    }
                    else
                    {
                        var query = $"USE {dataBase.name};";
                        Conn.Execute(query);

                        WriteLine($"Database \"{dataBase.name}\" opened for using.");
                    }
                }
                else
                {
                    bool isCorrectName = false;
                    string? name;

                    do
                    {
                        Write("Type new database name: ");
                        name = ReadLine();
                        if (baseList.FirstOrDefault(x => x.name.Equals(name)) is null)
                            isCorrectName = true;
                        else
                            WriteLine("Database with typed name exists. Try again.");
                    }
                    while (!isCorrectName);

                    var query = $"CREATE DATABASE {name};";
                    Conn.Execute(query);

                    query = $"USE {name};";
                    Conn.Execute(query);

                    WriteLine($"Database \"{name}\" opened for using.");
                }
            }
            catch (FormatException)
            {
                WriteLine("You didn't type proper base ID. Try again.");
                selectionOK = false;
            }
            catch (Exception exc)
            {
                WriteLine(exc.ToString());
                selectionOK = false;
            }
        }
        while (!selectionOK);
    }

    public static void ListAllTables()
    {
        string query = """
                    SELECT table_name, table_schema, table_type
                    FROM information_schema.tables
                    ORDER BY table_name ASC;
                    """;
        var result = Conn.Query(query);
        foreach (var row in result)
            WriteLine($"name: {row.table_name}, schema: {row.table_schema}, type: {row.table_type}");
    }

    public static void MultirowQuery()
    {
        throw new NotImplementedException();
    }

    public static void NonResultQuery()
    {
        throw new NotImplementedException();
    }
}
