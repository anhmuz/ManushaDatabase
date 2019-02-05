using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace ManushaDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "Manushi"
                };
                using (SqlConnection connection = new SqlConnection(builder.ToString()))
                {
                    connection.Open();
                    var db = new ManushaDatabase(connection);
                    db.Initialize();
                    db.InsertManusha("Manusha Hrusha");
                    db.InsertManusha("Manusha Kovbasenko", "high");
                    db.InsertCake("Brauni", 550);
                    db.InsertCake("Cheesecake", 400);
                    foreach (var manusha in db.ReadManushi())
                    {
                        Console.WriteLine(manusha.ToString());
                    }
                    foreach (var cake in db.ReadCakes())
                    {
                        Console.WriteLine(cake.ToString());
                    }
                    Console.WriteLine("Done");
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}
