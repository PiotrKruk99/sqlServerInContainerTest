using Dapper;
using System.Data.SqlClient;
using static System.Console;
using Models;

WriteLine("Sql Server console\n");

try
{
    DatabaseOper.conn.Open();

    IEnumerable<DatabaseDetails> baseList = DatabaseOper.GetDatabasesList();
    WriteLine("   0 - create and use new database\n");

    DatabaseOper.SelectDatabase(baseList);

    string appOptions = """

                Select option:
                   1 - non result query
                   2 - single row result query
                   3 - multirow result query
                   4 - list all tables
                   0 - exit
                   
                """;

    int selectedOption = -1;

    do
    {
        WriteLine(appOptions);

        try
        {
            selectedOption = Convert.ToInt32(ReadLine());
            
            switch (selectedOption)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 0:
                    break;
            }
        }
        catch (FormatException)
        {
            WriteLine("You didn't type proper option. Try again.");
        }
        catch (Exception exc)
        {
            WriteLine(exc.ToString());
        }
    }
    while (selectedOption != 0);

    DatabaseOper.conn.Close();
}
catch (SqlException exc)
{
    WriteLine("Sql error: " + exc.ToString());
}
catch (InvalidOperationException exc)
{
    WriteLine("Operator error: " + exc.ToString());
}
catch (Exception exc)
{
    WriteLine("Error: " + exc.ToString());
}
