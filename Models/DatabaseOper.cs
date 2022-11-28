using System.Data.SqlClient;
using static System.Console;
using Dapper;

namespace Models;

public static class DatabaseOper
{
    private static SqlConnectionStringBuilder connStr { get; }
    public static SqlConnection conn { get; }

    static DatabaseOper()
    {
        connStr = new SqlConnectionStringBuilder();
        connStr.ConnectionString = "Server=localhost;User Id=sa;" +
                "Password=MyVeryStrongPassword1";

        conn = new SqlConnection(connStr.ToString());
    }

    public static IEnumerable<DatabaseDetails> GetDatabasesList()
    {
        string query = """
                        SELECT name, database_id, create_date 
                        FROM sys.databases;
                        """;
        IEnumerable<DatabaseDetails> result = conn.Query<DatabaseDetails>(query);

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
                selectedId = Convert.ToInt32(ReadLine());
                DatabaseDetails? dataBase = baseList.FirstOrDefault(x => x.database_id == selectedId);

                if (dataBase is null)
                {
                    WriteLine("No base with typed ID. Try again");
                    selectionOK = false;
                }
                else
                {
                    var query2 = $"USE {dataBase.name};";
                    DatabaseOper.conn.Execute(query2);
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
}