using System.Data.SqlClient;
using LaptopShopApp.Models;
using LaptopShopApp.Services;

namespace LaptopShopApp.Repositories
{
    public class UserRepository : IRepository<User>
    {
        public List<User> GetAll()
        {
            var users = new List<User>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE IsActive = 1", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(MapUser(reader));
                    }
                }
            }
            return users;
        }

        public User? GetById(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserID", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
                    }
                }
            }
            return null;
        }

        public User? Authenticate(string username, string password)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
                    }
                }
            }
            return null;
        }

        public bool Add(User entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Users (Username, Password, Role, FullName, Email, IsActive) VALUES (@Username, @Password, @Role, @FullName, @Email, @IsActive)",
                    connection);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Password", entity.Password);
                command.Parameters.AddWithValue("@Role", entity.Role);
                command.Parameters.AddWithValue("@FullName", entity.FullName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(User entity)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Users SET Username = @Username, Password = @Password, Role = @Role, FullName = @FullName, Email = @Email, IsActive = @IsActive WHERE UserID = @UserID",
                    connection);
                command.Parameters.AddWithValue("@UserID", entity.UserID);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Password", entity.Password);
                command.Parameters.AddWithValue("@Role", entity.Role);
                command.Parameters.AddWithValue("@FullName", entity.FullName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET IsActive = 0 WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserID", id);
                return command.ExecuteNonQuery() > 0;
            }
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserID = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3),
                FullName = reader.GetString(4),
                Email = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                CreatedDate = reader.GetDateTime(6),
                IsActive = reader.GetBoolean(7)
            };
        }
    }
}
