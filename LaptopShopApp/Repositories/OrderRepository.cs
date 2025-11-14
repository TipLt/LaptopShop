using System.Data.SqlClient;
using LaptopShopApp.Models;
using LaptopShopApp.Services;

namespace LaptopShopApp.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        public List<Order> GetAll()
        {
            var orders = new List<Order>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Orders", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(MapOrder(reader));
                    }
                }
            }
            return orders;
        }

        public Order? GetById(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Orders WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapOrder(reader);
                    }
                }
            }
            return null;
        }

        public bool Add(Order entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Orders (CustomerID, TotalAmount, Status, Notes) VALUES (@CustomerID, @TotalAmount, @Status, @Notes); SELECT SCOPE_IDENTITY();",
                    connection);
                command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
                command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
                command.Parameters.AddWithValue("@Status", entity.Status);
                command.Parameters.AddWithValue("@Notes", entity.Notes);
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    entity.OrderID = Convert.ToInt32(result);
                    return true;
                }
                return false;
            }
        }

        public bool Update(Order entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Orders SET CustomerID = @CustomerID, TotalAmount = @TotalAmount, Status = @Status, Notes = @Notes WHERE OrderID = @OrderID",
                    connection);
                command.Parameters.AddWithValue("@OrderID", entity.OrderID);
                command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
                command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
                command.Parameters.AddWithValue("@Status", entity.Status);
                command.Parameters.AddWithValue("@Notes", entity.Notes);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Orders WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", id);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            var details = new List<OrderDetail>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM OrderDetails WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", orderId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        details.Add(new OrderDetail
                        {
                            OrderDetailID = reader.GetInt32(0),
                            OrderID = reader.GetInt32(1),
                            LaptopID = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            UnitPrice = reader.GetDecimal(4)
                        });
                    }
                }
            }
            return details;
        }

        public bool AddOrderDetail(OrderDetail detail)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO OrderDetails (OrderID, LaptopID, Quantity, UnitPrice) VALUES (@OrderID, @LaptopID, @Quantity, @UnitPrice)",
                    connection);
                command.Parameters.AddWithValue("@OrderID", detail.OrderID);
                command.Parameters.AddWithValue("@LaptopID", detail.LaptopID);
                command.Parameters.AddWithValue("@Quantity", detail.Quantity);
                command.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                return command.ExecuteNonQuery() > 0;
            }
        }

        private Order MapOrder(SqlDataReader reader)
        {
            return new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                Status = reader.GetString(4),
                Notes = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            };
        }
    }
}
