using LaptopShopApp.Models;
using LaptopShopApp.Repositories;

namespace LaptopShopApp.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        private static User? _currentUser;

        public AuthenticationService()
        {
            _userRepository = new UserRepository();
        }

        public static User? CurrentUser
        {
            get => _currentUser;
            private set => _currentUser = value;
        }

        public User? Login(string username, string password)
        {
            var user = _userRepository.Authenticate(username, password);
            if (user != null)
            {
                CurrentUser = user;
            }
            return user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool HasPermission(string action, string entity)
        {
            if (CurrentUser == null) return false;

            return CurrentUser.Role switch
            {
                "Admin" => true, // Admin has all permissions
                "Manager" => entity switch
                {
                    "Laptop" => true,
                    "Order" => action == "Read",
                    "Customer" => action == "Read",
                    _ => false
                },
                "Sales" => entity switch
                {
                    "Order" => action == "Create" || action == "Read",
                    "Customer" => action == "Read",
                    "Laptop" => action == "Read",
                    _ => false
                },
                "Warehouse" => entity switch
                {
                    "Laptop" => action == "Update" || action == "Read",
                    _ => false
                },
                _ => false
            };
        }
    }
}
