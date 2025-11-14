using System.Data.SqlClient;

namespace LaptopShopApp.Services
{
    // Singleton Pattern - Database Connection Manager
    public sealed class DatabaseConnection
    {
        private static DatabaseConnection? _instance;
        private static readonly object _lock = new object();
        private readonly string _connectionString;

        private DatabaseConnection()
        {
            // Default connection string - can be configured
            _connectionString = @"Server=DESKTOP-4LVS5AN\SQLEXPRESS;Database=LaptopShopDB;User=sa;Password=123456;TrustServerCertificate=True;";
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void SetConnectionString(string connectionString)
        {
            var field = typeof(DatabaseConnection).GetField("_connectionString", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(this, connectionString);
        }
    }
}
