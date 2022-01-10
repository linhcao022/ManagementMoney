using ManagementMoney.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ManagementMoney
{
        public static class DBConnect
        {
            public async static void InitializeDatabase()
            {
                await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteMoney.db",
                    Windows.Storage.CreationCollisionOption.OpenIfExists);

                string dbPath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteMoney.db");

                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath};Version=3"))
                {
                    await connection.OpenAsync();
                    String tableCommand = "CREATE TABLE IF NOT EXISTS Moneys (sid INTEGER PRIMARY KEY, MoneyCollected NVARCHAR(2048) NULL, Day NVARCHAR(2048) NULL, Spend NVARCHAR(2048) NULL)";
                    SQLiteCommand createTable = new SQLiteCommand(tableCommand, connection);

                    createTable.ExecuteReader();
                    connection.Close();
                }
            }

            public async static void AddData(string moneyCollected,string day, string spend)
            {
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteMoney.db");
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath};Version=3"))
                {
                    await connection.OpenAsync();

                    SQLiteCommand insertCommand = new SQLiteCommand();
                    insertCommand.Connection = connection;

                    insertCommand.CommandText = "INSERT INTO Moneys VALUES (NULL, @moneyCollected, @day, @spend);";
                    insertCommand.Parameters.AddWithValue("moneyCollected", moneyCollected);
                    insertCommand.Parameters.AddWithValue("day", day);
                    insertCommand.Parameters.AddWithValue("spend", spend);

                    insertCommand.ExecuteReaderAsync();

                    connection.Close();
                }
            }

            public async static
                Task LoadRecordAsync(ObservableCollection<Money> items)
            {
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteMoney.db");
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath};Version=3"))
                {
                    await connection.OpenAsync();

                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM Moneys", connection);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var moneyCollectedOrdinal = reader.GetOrdinal("MoneyCollected");
                        var dayOrdinal = reader.GetOrdinal("Day");
                        var spendOrdinal = reader.GetOrdinal("Spend");

                        while (await reader.ReadAsync())
                        {
                            string moneyCollected = reader.GetString(moneyCollectedOrdinal);
                            string day = reader.GetString(dayOrdinal);
                            string spend = reader.GetString(spendOrdinal);

                            items.Add(new Money(moneyCollected, day ,spend));
                        }
                    }
                }
            }
        
    }
}
