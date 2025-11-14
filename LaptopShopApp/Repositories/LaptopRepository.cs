using System.Data.SqlClient;
using LaptopShopApp.Models;
using LaptopShopApp.Services;

namespace LaptopShopApp.Repositories
{
    public class LaptopRepository : IRepository<Laptop>
    {
        public List<Laptop> GetAll()
        {
            var laptops = new List<Laptop>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Laptops", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        laptops.Add(MapLaptop(reader));
                    }
                }
            }
            return laptops;
        }

        public Laptop? GetById(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Laptops WHERE LaptopID = @LaptopID", connection);
                command.Parameters.AddWithValue("@LaptopID", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapLaptop(reader);
                    }
                }
            }
            return null;
        }

        public bool Add(Laptop entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Laptops (Brand, Model, Processor, RAM, Storage, GPU, Price, Stock, Description) " +
                    "VALUES (@Brand, @Model, @Processor, @RAM, @Storage, @GPU, @Price, @Stock, @Description)",
                    connection);
                AddParameters(command, entity);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(Laptop entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Laptops SET Brand = @Brand, Model = @Model, Processor = @Processor, RAM = @RAM, " +
                    "Storage = @Storage, GPU = @GPU, Price = @Price, Stock = @Stock, Description = @Description " +
                    "WHERE LaptopID = @LaptopID",
                    connection);
                command.Parameters.AddWithValue("@LaptopID", entity.LaptopID);
                AddParameters(command, entity);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Laptops WHERE LaptopID = @LaptopID", connection);
                command.Parameters.AddWithValue("@LaptopID", id);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateStock(int laptopId, int newStock)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Laptops SET Stock = @Stock WHERE LaptopID = @LaptopID", connection);
                command.Parameters.AddWithValue("@LaptopID", laptopId);
                command.Parameters.AddWithValue("@Stock", newStock);
                return command.ExecuteNonQuery() > 0;
            }
        }

        private void AddParameters(SqlCommand command, Laptop entity)
        {
            command.Parameters.AddWithValue("@Brand", entity.Brand);
            command.Parameters.AddWithValue("@Model", entity.Model);
            command.Parameters.AddWithValue("@Processor", entity.Processor);
            command.Parameters.AddWithValue("@RAM", entity.RAM);
            command.Parameters.AddWithValue("@Storage", entity.Storage);
            command.Parameters.AddWithValue("@GPU", entity.GPU);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@Stock", entity.Stock);
            command.Parameters.AddWithValue("@Description", entity.Description);
        }

        private Laptop MapLaptop(SqlDataReader reader)
        {
            return new Laptop
            {
                LaptopID = reader.GetInt32(0),
                Brand = reader.GetString(1),
                Model = reader.GetString(2),
                Processor = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                RAM = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Storage = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                GPU = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                Price = reader.GetDecimal(7),
                Stock = reader.GetInt32(8),
                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
            };
        }
    }
}
