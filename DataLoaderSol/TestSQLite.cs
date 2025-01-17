using System;
using System.Data.SQLite;

namespace TestAlpha
{
    public class TestSQLite
    {
        public static void Main(string[] args)
        {
            string databaseFile = "test.db";
            string connectionString = $"Data Source={databaseFile};Version=3;";

            // Create SQLite database file if it doesn't exist
            if (!System.IO.File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
                Console.WriteLine("SQLite database file created.");
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create a table
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS SampleTable (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT NOT NULL,
                                            Age INTEGER NOT NULL
                                         );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table created or already exists.");
                }

                // Insert sample data into the table
                string insertDataQuery = @"INSERT INTO SampleTable (Name, Age) VALUES
                                           ('Alice', 25),
                                           ('Bob', 30),
                                           ('Charlie', 22);";

                using (var command = new SQLiteCommand(insertDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Sample data inserted into the table.");
                }

                // Display the data
                string selectDataQuery = "SELECT * FROM SampleTable;";
                using (var command = new SQLiteCommand(selectDataQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Data in SampleTable:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Age: {reader["Age"]}");
                    }
                }

                connection.Close();
            }

            Console.WriteLine("SQLite operations completed successfully.");
        }
    }
}
