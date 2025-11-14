using System.Data.SqlClient;
using LaptopShopApp.Models;
using LaptopShopApp.Services;

namespace LaptopShopApp.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        public List<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Customers", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(MapCustomer(reader));
                    }
                }
            }
            return customers;
        }

        public Customer? GetById(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Customers WHERE CustomerID = @CustomerID", connection);
                command.Parameters.AddWithValue("@CustomerID", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }
            return null;
        }

        public bool Add(Customer entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Customers (CustomerName, Email, Phone, Address) VALUES (@CustomerName, @Email, @Phone, @Address)",
                    connection);
                command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@Phone", entity.Phone);
                command.Parameters.AddWithValue("@Address", entity.Address);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(Customer entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Customers SET CustomerName = @CustomerName, Email = @Email, Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID",
                    connection);
                command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
                command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@Phone", entity.Phone);
                command.Parameters.AddWithValue("@Address", entity.Address);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID", connection);
                command.Parameters.AddWithValue("@CustomerID", id);
                return command.ExecuteNonQuery() > 0;
            }
        }

        private Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Phone = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                CreatedDate = reader.GetDateTime(5)
            };
        }
    }
}
